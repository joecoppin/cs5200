using System;
using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class Resource
    {
        [DataMember]
        public Guid SerialNumber { get; set; }

        [DataMember]
        public DateTime BuildStartTime { get; set; }

        [DataMember]
        public DateTime BuildEndTime { get; set; }
    }
}
