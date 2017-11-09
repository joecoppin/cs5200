using CommSubsystem;
using CommSubsystem.Conversations;
using Messages;
using SharedObjects;

namespace SharedAppLayer.Conversations
{
    public class DiscoveryOfServerResponder : ResponderConversation
    {
        public static int SuccesCount { get; private set; }

        public static int FailureCount { get; private set; }

        protected override void ExecuteDetails()
        {
            if (!IsEnvelopeValid(FirstEnvelope, typeof(ServerDiscoveryMessage)))
            {
                Error = new Error()
                {
                    Text =  $"Wrong kind of message, excepted {typeof(ServerDiscoveryMessage)}"
                };
                FailureCount++;
                return;
            }

            var reply = new ServerAliveMessage();
            reply.SetMessageAndConversationIds(MessageId.Create(), FirstEnvelope.Message.ConvId);
            var env = new Envelope() {Message = reply, EndPoint = FirstEnvelope.EndPoint};
            Error = CommFacility.Send(env);
            if (Error != null)
                SuccesCount++;
            else
                FailureCount++;
        }
    }
}
