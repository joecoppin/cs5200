using CommSubsystem;
using DataServer.Conversations;
using Messages;
using SharedAppLayer.Conversations;

namespace DataServer
{
    public class DataServerConversationFactory : ConversationFactory
    {
        public override void Initialize()
        {
            AddOutgoingMapping(typeof(ServerDiscoveryMessage), typeof(DiscoveryOfServerInitiator));

            AddIncomingMapping(typeof(WeatherDataMessage), typeof(PublishWeatherDataResponder));
        }
    }
}
