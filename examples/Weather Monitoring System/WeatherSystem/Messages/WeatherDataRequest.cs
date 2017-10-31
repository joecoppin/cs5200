using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    [DataContract]
    public class WeatherDataRequest : Message
    {
        [DataMember]
        public DateRange SelectedDateRange { get; set; }
        [DataMember]
        public Location GeoLocation { get; set; }
    }
}
