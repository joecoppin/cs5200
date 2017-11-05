using System.Net;
using SharedObjects;

namespace CommSubsystem.Conversations
{
    public abstract class ResponderConversation : Conversation
    {
        protected IPEndPoint RemoteEndPoint { get; set; }

        protected override bool Initialize()
        {
            RemoteEndPoint = FirstEnvelope?.EndPoint;

            return true;
        }
    }
}
