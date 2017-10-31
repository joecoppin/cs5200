using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Messages
{
    [DataContract]
    public class ReserveResource : ControlMessage
    {
        [DataMember]
        public int NumberToReserve { get; set; }
    }
}
