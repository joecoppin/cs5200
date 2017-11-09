using System.Net;

namespace SharedAppLayer
{
    public class ServerInfo
    {
        public short ProcessId { get; set; }
        public IPEndPoint EndPoint { get; set; }

        public ServerInfo Clone()
        {
            return new ServerInfo() {ProcessId = ProcessId, EndPoint = EndPoint};
        }
    }
}
