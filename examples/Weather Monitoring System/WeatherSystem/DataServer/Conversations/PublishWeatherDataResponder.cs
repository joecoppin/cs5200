using CommSubsystem.Conversations;
using Messages;
using SharedObjects;

namespace DataServer.Conversations
{
    public class PublishWeatherDataResponder : ResponderConversation
    {
        public static int SuccesCount { get; private set; }

        public static int FailureCount { get; private set; }

        protected override void ExecuteDetails()
        {
            if (!IsEnvelopeValid(FirstEnvelope, typeof(WeatherDataMessage)))
            {
                Error = new Error()
                {
                    Text = $"Wrong kind of message, excepted {typeof(ServerDiscoveryMessage)}"
                };
                FailureCount++;
                return;
            }

            var msg = FirstEnvelope.Message as WeatherDataMessage;
            SuccessAction?.Invoke(msg?.Data);
            SuccesCount++;
        }
    }
}
