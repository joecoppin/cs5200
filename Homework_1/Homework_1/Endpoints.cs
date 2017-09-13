using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Server_Client
{
    class Endpoints
    {
        public static IPEndPoint Parse(string hostNamePort)
        {
            IPEndPoint result = null;
            if (!string.IsNullOrWhiteSpace(hostNamePort))
            {
                string[] tmp = hostNamePort.Split(':');
                if (tmp.Length == 2 && !string.IsNullOrWhiteSpace(tmp[0]) && !string.IsNullOrWhiteSpace(tmp[1]))
                    result = new IPEndPoint(ParseAddress(tmp[0]), ParsePort(tmp[1]));
            }
            return result;
        }

        public static IPAddress ParseAddress(string hostname)
        {
            IPAddress result = null;
            IPAddress[] addressList = Dns.GetHostAddresses(hostname);
            for (int i = 0; i < addressList.Length && result == null; i++)
                if (addressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    result = addressList[i];
            return result;
        }

        public static int ParsePort(string portStr)
        {
            int port = 0;
            if (!string.IsNullOrWhiteSpace(portStr))
                Int32.TryParse(portStr, out port);
            return port;
        }
    }
}
