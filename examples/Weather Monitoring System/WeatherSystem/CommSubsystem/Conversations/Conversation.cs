using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using SharedObjects;

namespace CommSubsystem.Conversations
{
    public abstract class Conversation
    {
        public delegate void ActionHandler(object context);

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Conversation));

        /// <summary>
        /// This is CommFacility managing this conversation
        /// </summary>
        public CommFacility CommFacility { get; set; }

        public Envelope FirstEnvelope { get; set; }

        /// <summary>
        /// Hold the identitifier for this conversation's queue
        /// </summary>
        public MessageId ConvId => FirstEnvelope?.Message?.ConvId;

        /// <summary>
        /// The context can hold any information that is needed for a concrete converstion
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// This is the conversation's current status.  Conversations start out as NotInitialized.
        /// Then, once started the becoming "Working".   From there, they terminate as either "Failed"
        /// or "Succeeded"
        /// </summary>
        public enum PossibleStatus
        {
            NotInitialized,
            Initializing,
            Working,
            Failed,
            Succeeded
        };
        public PossibleStatus Status { get; protected set; } = PossibleStatus.NotInitialized;

        /// <summary>
        /// If an error occurs during the conversation, it should be saved in this data member
        /// </summary>Con
        public Error Error { get; protected set; }

        /// <summary>
        /// A delegate to some function that will be called before initialization.  If no
        /// function needs to be called, leave this null.
        /// </summary>
        public ActionHandler PreInitializationAction { get; set; }

        /// <summary>
        /// A delegate to some function that will be called before initialization.  If no function needs to
        /// be called, leave this null.
        /// </summary>
        public ActionHandler PreExecuteAction { get; set; }

        /// <summary>
        /// A delegate to some function that will just after the conversation has entered the Success state
        /// If no function needs to be called, leave this null.
        /// </summary>
        public ActionHandler SuccessAction { get; set; }

        /// <summary>
        /// A delegate to some function that will just after the conversation has entered the Failure state
        /// If no function needs to be called, leave this null.
        /// </summary>
        public ActionHandler FailureAction { get; set; }

        /// <summary>
        /// A delagate to some function that will be called after execution. In no function needs
        /// to be called, leave this blank.
        /// </summary>
        public ActionHandler PostExecuteAction { get; set; }

        /// <summary>
        /// For conversation that recieves messages, this is the amount of time to await for an incoming message
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// For conversation that can resend and retry the waiting for a reply, this is the maximum number of retries
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        /// This is set to true when the conversation finishes
        /// </summary>
        public bool Done => Status == PossibleStatus.Succeeded || Status == PossibleStatus.Failed;


        /// <summary>
        /// This is the main method for executing a conversation.  If you want to execute the conversation
        /// synchronously, call this method directly.  If you want to execute the conversation asynchronously
        /// call the Launch method, which will run it on a thread.
        /// 
        /// Note that this methods leverages both the Strategy and Template Method design patterns.
        /// </summary>
        public virtual void Execute()
        {
            if (FirstEnvelope == null)
            {
                Error = new Error() {Text = "No First Envelope specified"};
                Logger.Warn(Error.Text);
                return;
            }

            PreInitializationAction?.Invoke(Context);

            Status = PossibleStatus.Initializing;

            SetupConversationQueue();

            if (Initialize())
            {
                PreExecuteAction?.Invoke(Context);
                Status = PossibleStatus.Working;
                ExecuteDetails();                   // Application specific logic for this conversation.
                                                    // This method will set the Error property if there
                                                    // was an error during execution of the conversation.
            }

            Status = (Error == null) ? PossibleStatus.Succeeded : PossibleStatus.Failed;

            if (Error != null)
                Logger.Warn(Error.Text);

            PostExecuteAction?.Invoke(Context);
        }

        /// <summary>
        /// This method runs the conversation on its own thread.
        /// </summary>
        /// <param name="context"></param>
        public void Launch(object context = null)
        {
            Context = context;
            Task.Factory.StartNew(Execute);
            Logger.DebugFormat($"Launched {GetType().Name}");
        }

        /// <summary>
        /// This method places an incoming envelope into a conversation's own dedicated envelope queue.
        /// The conversation will process that envelope when its ready to do so.
        /// </summary>
        /// <param name="env"></param>
        public void Enqueue(Envelope env)
        {
            ConvQueue?.Enqueue(env);
        }

        #region Protected and Private Methods

        protected virtual bool UsesConverstationQueue => true;

        protected abstract bool Initialize();

        protected EnvelopeQueue ConvQueue { get; set; }
    
        protected void SetupConversationQueue()
        {
            if (UsesConverstationQueue)
            {
                Logger.DebugFormat($"Setup conversation's envelope queue {ConvId}");
                CommFacility.CreateConversationQueue(ConvId, this);
            }
            else
                Logger.Debug("No conversation queue needed");
        }

        protected abstract void ExecuteDetails();

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
                var messageType = env.Message.GetType();

                Logger.DebugFormat($"See if {messageType} is valid for a {GetType().Name} conversation");
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

            var remainingSends = MaxRetries;
            while (remainingSends > 0 && incomingEnvelope == null)
            {
                Error = CommFacility.Send(outgoingEnv);
                remainingSends--;

                if (Error != null) break;       // If there an error with the send, don't try to recieve

                incomingEnvelope = ConvQueue.Dequeue(Timeout);
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
