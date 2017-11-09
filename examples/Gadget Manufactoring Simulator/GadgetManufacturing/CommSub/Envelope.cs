using System.Net;

using Messages;
using SharedObjects;

namespace CommSub
{
    public class Envelope
    {
        public ControlMessage Message { get; set; }
        
        public PublicEndPoint EndPoint { get; set; }

        public Envelope() {}

        public Envelope(ControlMessage message, PublicEndPoint endPoint)
        {
            Message = message;
            EndPoint = endPoint;
        }

        public Envelope(ControlMessage message, IPEndPoint ep) :
            this(message, (ep != null) ? new PublicEndPoint() { IpEndPoint = ep } : null) {}

        public IPEndPoint IpEndPoint
        {
            get
            {
                return (EndPoint == null) ?
                    new IPEndPoint(IPAddress.Any, 0) :
                    EndPoint.IpEndPoint;
            }
            set { EndPoint = (value == null) ? null : new PublicEndPoint() { IpEndPoint = value }; }
        }

        public bool IsValidToSend => (Message != null &&
                                      EndPoint != null &&
                                      EndPoint.Host!="0.0.0.0" &&
                                      EndPoint.Port!=0);
    }
}
