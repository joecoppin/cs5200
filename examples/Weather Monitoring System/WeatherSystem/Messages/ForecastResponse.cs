using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    [DataContract]
    public class ForecastResponse : Message
    {
        [DataMember]
        public WeatherData[] ForecastData { get; set; }
    }
}
