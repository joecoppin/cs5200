using CommSubsystem.Conversations;

namespace WeatherStation.Conversations
{
    public class PublishWeatherDataInitiator : InitiatorConversation
    {
        public static int SuccesCount { get; private set; }

        public static int FailureCount { get; private set; }

        protected override void ExecuteDetails()
        {
            Error = CommFacility.Send(FirstEnvelope);
            if (Error != null)
                SuccesCount++;
            else
                FailureCount++;
        }
    }
}
