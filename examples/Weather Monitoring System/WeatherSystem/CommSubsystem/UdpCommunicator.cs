using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommSubsystem
{
    public class UdpCommunicator
    {
        protected UdpClient MyUdpClient;
        private Task _myTask;
        private bool _keepGoing;

        public IPAddress GroupAddress { get; set; }
        public int GroupPort { get; set; }
        public int TimeoutInMilliseconds { get; set; }

    }
}
