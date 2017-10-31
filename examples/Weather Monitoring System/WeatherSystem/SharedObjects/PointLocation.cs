using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class PointLocation : Location
    {
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public double Latitude { get; set; }
    }
}
