using System.Runtime.Serialization;

using SharedObjects;

namespace Messages
{
    [DataContract]
    public class Nak : ControlMessage
    {
        [DataMember]
        public Error Error { get; set; }
    }
}
