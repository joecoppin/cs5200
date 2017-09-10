using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

using log4net;

namespace SimpleUDPSocket
{
    public class SimpleSender
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SimpleReceiver));

        private static int _nextId = 1;
        private readonly UdpClient _myUdpClient;

        public List<IPEndPoint> Peers { get; set; }

        public SimpleSender()
        {
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
            _myUdpClient = new UdpClient(localEp);
            Peers = new List<IPEndPoint>();
        }

        public void SendStuff()
        {
            bool keepGoing = true;

            while (keepGoing)
            {
                Console.Write("A=Add Peer, S=Send Message, or EXIT: " );
                string cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd))
                    continue;

                switch (cmd.Trim().ToUpper())
                {
                    case "A":
                        AddPeer();
                        break;
                    case "S":
                        AskForMessageAndSend();
                        break;
                    case "EXIT":
                        SendToPeers(cmd);
                        keepGoing = false;
                        break;
                }
            }
        }

        private void AddPeer()
        {
            Console.Write("Enter Peer EP (host:port): ");
            string peer = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(peer))
            {
                IPEndPoint peerAddress = EndPointParser.Parse(peer);
                if (peerAddress != null)
                {
                    Peers.Add(peerAddress);
                    Logger.DebugFormat("Add {0} as a peer", peerAddress);
                }
            }
        }

        private void AskForMessageAndSend()
        {
            Console.Write("Enter a message to send: ");
            string message = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(message))
                SendToPeers(message);
        }

        private void SendToPeers(string message)
        {
            if (Peers != null && Peers.Count > 0)
            {
                Message msg = new Message()
                {
                    Id = GetNextId(),
                    Timestamp = DateTime.Now,
                    Text = message
                };

                byte[] bytes = msg.Encode();

                foreach (IPEndPoint ep in Peers)
                {
                    int bytesSent = _myUdpClient.Send(bytes, bytes.Length, ep);
                    Logger.InfoFormat("Send to {0} was {1}", ep, (bytesSent == bytes.Length) ? "Successful" : "Not Successful");
                }
            }
        }

        private static int GetNextId()
        {
            if (_nextId == int.MaxValue)
                _nextId = 1;
            return _nextId++;
        }
    }
}
