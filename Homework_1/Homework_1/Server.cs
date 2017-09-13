

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server_Client
{
    class Server
    {
        UdpClient udp_client;
        public Server()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            udp_client = new UdpClient(endPoint);
            udp_client.Client.ReceiveTimeout = 1000;
        }

        public IPEndPoint local_endpoint
        {
            get
            {
                IPEndPoint result = null;
                if (udp_client != null)
                {
                    result = udp_client.Client.LocalEndPoint as IPEndPoint;
                }
                return result;
            }
        }

        public void Run()
        {
            Console.WriteLine("Server Running");
            bool quit = false;
            while (!quit)
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] bytes = null;

                try
                {
                    bytes = udp_client.Receive(ref remoteEp);
                }
                catch (SocketException error)
                {
                    if (error.SocketErrorCode != SocketError.TimedOut)
                        throw;
                }

            }

        }

        void GameDef(IPEndPoint hold)
        {
           
            Console.WriteLine("In GameDef Function");
            //how to find hint or definition?
            short id = 1;
            string myhint = "hint_hc";
            string mydef = "def";
            Message message = new Message()
            {
                MessageType = 2,
                game_id = id,
                hint = myhint,
                def = mydef
            };

            byte[] bytes = message.Encode();
            
            int bytesSent = udp_client.Send(bytes, bytes.Length, hold);
            
        }

        void Answer(IPEndPoint hold)
        {
           
            Console.WriteLine("In Answer Function");
            short id = 1;
            byte res = 1;
            short myscore = 0;
            string myhint = "hard_coded";
            Message message = new Message()
            {
                MessageType = 4,
                game_id = id,
                result = res,
                score = myscore,
                hint = myhint,
            };

            byte[] bytes = message.Encode();

            int bytesSent = udp_client.Send(bytes, bytes.Length, hold);

        }

        void Hint(IPEndPoint hold)
        {
       
            Console.WriteLine("In Hint Function");
            short id = 1;
            string myhint = "hard_coded";
            Message message = new Message()
            {
                MessageType = 6,
                game_id = id,
                hint = myhint,
            };

            byte[] bytes = message.Encode();

            int bytesSent = udp_client.Send(bytes, bytes.Length, hold);
        }

        void Error(IPEndPoint hold)
        {
          
            Console.WriteLine("In Error Function");

            short id = 1;
            string myhint = "hard_coded";
            Message message = new Message()
            {
                MessageType = 9,
                game_id = id,
                error = myhint,
            };

            byte[] bytes = message.Encode();

            int bytesSent = udp_client.Send(bytes, bytes.Length, hold);
        }

        void Heartbeat(IPEndPoint hold)
        {
            /*
            * The message's bytes will contain encodings of the following, in order:
            * 
            * Message Type = 10   short integer
            * Game Id             short integer
            * 
            * */
            Console.WriteLine("In Heartbeat Function");

            short id = 10;
            Message message = new Message()
            {
                MessageType = 10,
                game_id = id,
            };

            byte[] bytes = message.Encode();

            int bytesSent = udp_client.Send(bytes, bytes.Length, hold);
        }
    }
}


    