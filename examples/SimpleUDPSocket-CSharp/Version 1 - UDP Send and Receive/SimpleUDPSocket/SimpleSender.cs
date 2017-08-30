﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace SimpleUDPSocket
{
    public class SimpleSender
    {
        private readonly UdpClient _myUdpClient;

        public List<IPEndPoint> Peers { get; set; }

        public SimpleSender()
        {
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
            _myUdpClient = new UdpClient(localEp);
            Peers = new List<IPEndPoint>();
        }

        public void DoStuff()
        {
            string cmd = string.Empty;
            while (string.IsNullOrEmpty(cmd) || cmd.Trim().ToUpper() != "EXIT")
            {
                Console.Write("A=Add Peer, S=Send Message, or EXIT: " );
                cmd = Console.ReadLine();
                if (cmd != null)
                {
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
                            break;
                    }
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
                if (peerAddress!=null)
                    Peers.Add(peerAddress);
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
            if (Peers!=null && Peers.Count>0)
                foreach (IPEndPoint ep in Peers)
                {
                    byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
                    int result = _myUdpClient.Send(sendBuffer, sendBuffer.Length, ep);
                    Console.WriteLine("Send result = {0}", result);
                }
        }
    }
}
