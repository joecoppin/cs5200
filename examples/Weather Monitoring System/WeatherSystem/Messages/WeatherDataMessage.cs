using System.Collections.Generic;
using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    public class WeatherDataMessage : Message
    {
        public int Count { get; set; }
        [DataMember]
        public List<WeatherData> ForecastData { get; set; }
    }
}
