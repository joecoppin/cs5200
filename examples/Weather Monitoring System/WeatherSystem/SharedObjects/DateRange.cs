using System;
using System.Runtime.Serialization;

namespace SharedObjects
{
    [DataContract]
    public class DateRange
    {
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }
    }
}
