using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class CircleLocation : Location
    {
        [DataMember]
        public double Radius { get; set; }
    }
}
