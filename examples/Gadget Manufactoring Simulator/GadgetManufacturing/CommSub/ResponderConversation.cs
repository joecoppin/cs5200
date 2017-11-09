using SharedObjects;

namespace CommSub
{
    public abstract class ResponderConversation : Conversation
    {
        /// <summary>
        /// For conversations started by an incoming message, this is the message
        /// </summary>
        public Envelope IncomingEnvelope { get; set; }

        protected override bool Initialize()
        {
            ConvId = IncomingEnvelope?.Message?.ConvId;
            if (ConvId != null)
            {
                SetupConversationQueue();
                RemotEndPoint = IncomingEnvelope?.EndPoint;
                State = PossibleState.Working;
            }
            else
                Error = new Error() { Text = $"Cannot initialize {GetType().Name} conversation because ConvId in incoming message is null" };

            return (Error == null);
        }
    }
}
