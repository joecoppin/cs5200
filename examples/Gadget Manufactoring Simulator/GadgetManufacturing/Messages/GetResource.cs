using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    [DataContract]
    public class GetResource : ControlMessage
    {
        [DataMember]
        public ResourceType ResourceType { get; set; }

        [DataMember]
        public int NumberNeeded { get; set; }

        [DataMember]
        public int TargetMilliseconds { get; set; }
    }
}
