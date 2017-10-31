using System.Runtime.Serialization;

namespace Messages
{
    [DataContract]
    public class ProgressStatus : Message
    {
        [DataMember]
        public float PercentComplete { get; set; }
    }
}
