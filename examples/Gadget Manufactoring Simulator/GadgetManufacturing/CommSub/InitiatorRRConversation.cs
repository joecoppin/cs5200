using System;
using log4net;
using Messages;
using SharedObjects;

namespace CommSub
{
    // ReSharper disable once InconsistentNaming
    public abstract class InitiatorRRConversation : InitiatorConversation
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InitiatorRRConversation));

        protected override void ExecuteDetails(object context)
        {
            Envelope env = ReliableSend(FirstEnvelope, ExceptedReplyType, typeof(Nak));
            Logger.Debug("Back from ReliableSend with");

            if (env == null)
                Error = new Error() { Text = "No response received" };
            else if (env.Message is Nak)
                Error = ((Nak)env.Message).Error;
            else
                ProcessValidResponse(env);
        }

        protected abstract Type ExceptedReplyType { get; }

        protected abstract void ProcessValidResponse(Envelope env);
    }
}
