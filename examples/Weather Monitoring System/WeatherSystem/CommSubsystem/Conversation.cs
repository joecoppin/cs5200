using System;
using System.Linq;
using System.Net;
using System.Threading;

using SharedObjects;

using log4net;

namespace CommSubsystem
{
    public abstract class Conversation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Conversation));

        public enum PossibleState
        {
            NotInitialized,
            Working,
            Failed,
            Successed
        };
        public PossibleState State { get; protected set; } = PossibleState.NotInitialized;

        /// <summary>
        /// This is CommFacility managing this conversation
        /// </summary>
        public CommFacility CommFacility { get; set; }
        public IPEndPoint RemotEndPoint { get; set; }

        /// <summary>
        /// Hold the identitifier for this conversation's queue
        /// </summary>
        public MessageId ConvId { get; protected set; }

        /// <summary>
        /// If an error occurs during the conversation, it should be saved in this data member
        /// </summary>
        public Error Error { get; protected set; }

        public delegate void ActionHandler(object context);

        public ActionHandler PreExecuteAction { get; set; }
        public ActionHandler PostExecuteAction { get; set; }

        /// <summary>
        /// For conversations that will have a timeout, this is the timeout value in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// For conversation that can resend and retry the waiting for a reply, this is the maximum number of retries
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        /// This is set to true when the conversation finishes
        /// </summary>
        public bool Done { get; protected set; }

        public void Launch(object context = null)
        {
            var result = ThreadPool.QueueUserWorkItem(Execute, context);
            Logger.DebugFormat($"Launch of {GetType().Name}, result = {result}");
        }

        public virtual void Execute(object context = null)
        {
            PreExecuteAction?.Invoke(context);

            if (Initialize())
                ExecuteDetails(context);

            if (Error == null)
                State = PossibleState.Successed;
            else
            {
                State = PossibleState.Failed;
                Logger.Warn(Error.Text);
            }

            PostExecuteAction?.Invoke(context);
        }

        public void Enqueue(Envelope env)
        {
            MyQueue?.Enqueue(env);
        }

        protected virtual bool UsesConverstationQueue => true;

        protected abstract bool Initialize();

        protected abstract void ExecuteDetails(object context);

        protected EnvelopeQueue MyQueue { get; set; }

        protected void SetupConversationQueue()
        {
            if (UsesConverstationQueue)
            {
                Logger.DebugFormat($"Setup conversation's envelope queue {ConvId}");
                MyQueue = new EnvelopeQueue();
            }
            else
                Logger.Debug("No conversation queue needed");
        }

        #region Private and Protected Methods
        protected bool IsEnvelopeValid(Envelope env, params Type[] allowedTypes)
        {
            Error = null;
            Logger.Debug("Checking to see if envelope is valid and message of appropriate type");
            if (env?.Message == null)
                Error = new Error() { Text = "Null or empty message" };
            else if (env.Message.MsgId == null)
                Error = new Error() { Text = "Null Message Number" };
            else if (env.Message.ConvId == null)
                Error = new Error() { Text = "Null Conversation Id" };
            else
            {
                Type messageType = env.Message.GetType();

                Logger.DebugFormat("See if {0} is valid for a {1} conversation", messageType, GetType().Name);
                if (!allowedTypes.Contains(messageType))
                {
                    Error = new Error()
                    {
                        Text = "Invalid Type of Message. Allow Types: " +
                                                allowedTypes.Aggregate(string.Empty, (current, t) => current + t.ToString())
                    };
                }
            }

            if (Error != null)
                Logger.Error(Error.Text);

            return (Error == null);
        }

        protected Envelope ReliableSend(Envelope outgoingEnv, params Type[] allowedTypes)
        {
            Envelope incomingEnvelope = null;

            int remainingSends = MaxRetries;
            while (remainingSends > 0 && incomingEnvelope == null)
            {
                Error = CommFacility.Send(outgoingEnv);
                remainingSends--;

                if (Error != null) break;       // If there was a send, error don't try to recieve

                incomingEnvelope = MyQueue.Dequeue(Timeout);
                if (!IsEnvelopeValid(incomingEnvelope, allowedTypes))
                    incomingEnvelope = null;
            }

            if (Error != null)
                Logger.Error(Error.Text);

            return incomingEnvelope;
        }

        #endregion
    }
}
