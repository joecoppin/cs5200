using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    [DataContract]
    public class ForecastRequest : Message
    {
        [DataMember]
        public DateRange SelectedDateRange { get; set; }
        [DataMember]
        public Location GeoLocation { get; set; }

    }
}
