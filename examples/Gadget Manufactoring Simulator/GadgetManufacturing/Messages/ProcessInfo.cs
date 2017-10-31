using System.Runtime.Serialization;

namespace Messages
{
    [DataContract]
    public class ProcessInfo : ControlMessage
    {
        [DataMember]
        public short ProcessId { get; set; }
        [DataMember]
        public string GroupMulticast { get; set; }
    }
}
