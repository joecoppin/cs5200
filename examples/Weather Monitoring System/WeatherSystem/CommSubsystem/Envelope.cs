using System.Net;

using Messages;

namespace CommSubsystem
{
    public class Envelope
    {
        public Message Message { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public Envelope() { }

        public Envelope(Message message, IPEndPoint endPoint)
        {
            Message = message;
            EndPoint = endPoint;
        }

        public IPEndPoint IpEndPoint
        {
            get
            {
                return EndPoint ?? new IPEndPoint(IPAddress.Any, 0);
            }
            set { EndPoint = value; }
        }

        public bool IsValidToSend => (Message != null &&
                                      EndPoint != null &&
                                      EndPoint.Address.ToString() != "0.0.0.0" &&
                                      EndPoint.Port != 0);
    }
}
