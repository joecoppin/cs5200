using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    [DataContract]
    public class Register : ControlMessage
    {
        [DataMember]
        public ProcessType ProcessType { get; set; }
    }
}
