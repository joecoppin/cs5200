using System.ComponentModel;
using System.Runtime.Serialization;

namespace Messages
{
    [DataContract]
    public class DataChannelInfoMessage : Message
    {
        [DataMember]
        public int PortNumber { get; set; }
    }
}
