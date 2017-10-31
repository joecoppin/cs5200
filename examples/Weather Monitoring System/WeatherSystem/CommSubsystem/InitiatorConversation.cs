using Messages;
using SharedObjects;

namespace CommSubsystem
{
    public abstract class InitiatorConversation : Conversation
    {
        protected Envelope FirstEnvelope { get; set; }

        protected override bool Initialize()
        {
            FirstEnvelope = null;

            ConvId = MessageId.Create();
            SetupConversationQueue();
            State = PossibleState.Working;

            var msg = CreateFirstMessage();
            if (msg != null)
            {
                msg.SetMessageAndConversationIds(ConvId);
                FirstEnvelope = new Envelope() { Message = msg, EndPoint = RemotEndPoint };
            }
            return (FirstEnvelope != null);
        }

        protected abstract Message CreateFirstMessage();

    }
}
