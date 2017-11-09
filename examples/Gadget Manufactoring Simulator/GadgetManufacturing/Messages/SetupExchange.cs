using System.Runtime.Serialization;
using SharedObjects;

namespace Messages
{
    [DataContract]
    public class SetupExchange : ControlMessage
    {
        [DataMember]
        public PublicEndPoint TcpEndPoint { get; set; }
    }
}
