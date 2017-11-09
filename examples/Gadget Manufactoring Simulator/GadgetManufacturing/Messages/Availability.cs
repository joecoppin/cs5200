using System.Runtime.Serialization;

namespace Messages
{
    [DataContract]
    public class Availability : ControlMessage
    {
        [DataMember]
        public int Available { get; set; }
    }
}
