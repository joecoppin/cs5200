using System;
using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class Error
    {
        [DataMember]
        public string Text { get; set; }
        
        [DataMember]
        public DateTime Timestamp { get; private set; } = DateTime.Now;
    }
}

