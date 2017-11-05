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

        public bool IsValidToSend => (Message != null &&
                                      EndPoint != null &&
                                      EndPoint.Address.ToString() != "0.0.0.0" &&
                                      EndPoint.Port != 0);
    }
}
