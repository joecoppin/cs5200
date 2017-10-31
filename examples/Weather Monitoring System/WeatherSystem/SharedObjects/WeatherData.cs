using System;
using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class WeatherData
    {
        [DataMember]
        public Location SubjectLocation { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public double Temperature { get; set; }
        [DataMember]
        public double WindDirection { get; set; }
        [DataMember]
        public double WindSpeed { get; set; }
        [DataMember]
        public double BarometricPressure { get; set; }
        [DataMember]
        public double Precipitation { get; set; }
        [DataMember]
        public double Humidity { get; set; }
        [DataMember]
        public double Visibility { get; set; }
    }
}
