using CommSubsystem;
using Messages;
using SharedAppLayer.Conversations;
using WeatherStation.Conversations;

namespace WeatherStation
{
    public class WeatherStationConversationFactory : ConversationFactory
    {
        public override void Initialize()
        {
            AddOutgoingMapping(typeof (ServerDiscoveryMessage), typeof (DiscoveryOfServerInitiator));
            AddOutgoingMapping(typeof(WeatherDataMessage), typeof(PublishWeatherDataInitiator));
        }
    }
}
