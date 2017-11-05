using System;
using System.Threading;
using CommSubsystem;
using SharedAppLayer;

namespace WeatherStation
{
    public class WeatherStationWorker : WmsAppWorker
    {
        private DateTime _lastPublication = DateTime.MinValue;
        
        protected override void Run()
        {
            while (KeepGoing)
            {
                DataServerDiscovery();
                PublishWeatherData();
                Thread.Sleep(100);
            }
        }

        protected override ConversationFactory CreateConversationFactory()
        {
            return new WeatherStationConversationFactory();
        }

        protected void PublishWeatherData()
        {
            var options = Options as WeatherStationRuntimeOptions;
            var currentTime = DateTime.Now;
            if (currentTime.Subtract(_lastPublication).Minutes > options?.PublicationInterval)
            {


                _lastPublication = currentTime;
            }

        }

    }
}
