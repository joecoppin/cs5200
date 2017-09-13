
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace Server_Client
{
    class Player
    {

        private readonly UdpClient udp_client;
        //IPEndPoint serverEndpoint = new IPEndPoint();

        public List<IPEndPoint> Peers { get; set; }

        public Player()
        {
            IPEndPoint local_endpoint = new IPEndPoint(IPAddress.Any, 0);
            udp_client = new UdpClient(local_endpoint);
            //ipenpoint
            //
            //Peers = new List<IPEndPoint>();
        }

        public void Display()
        {
            Console.WriteLine("(N)ew Game");
            Console.WriteLine("(H)int");
            Console.WriteLine("(G)uess");
            Console.WriteLine("(Q)uit");

        }

        public void GetUserInput(){
			Display();
			string input = Console.ReadLine();
			if (!string.IsNullOrEmpty(input) && input.Length > 0)
			{
				switch (input.Trim().ToUpper().Substring(0, 1))
				{
					case "N":
						NewGame();
						break;
					case "H":
						Hint();
						break;
					case "G":
						Guess();
						break;
					case "Q":
						Exit();
						break;
				}
			}
        }

        public Boolean HandleIncommingCom(){
            //read socket
            //decode
            //handle 
            //return true if data
            //return false if nothing
        }

        public void Run()
        {
            bool run = true;

            while (run)
            {
                if (!HandleIncommingCom())
                {
                    //potential problem with using only one thread
                    GetUserInput();
                }
            }
        }

        private void NewGame()
        {
            Console.WriteLine("In NewGame Function");
            Message message = new Message()
            {
                MessageType = 1,
                a_num = "a01499868_hardcoded",
                l_name = "coppin_hc",
                f_name = "joe_hc",
                alias = "jc"
            };

            byte[] bytes = message.Encode();


            int bytesSent = udp_client.Send(bytes, bytes.Length, ep);
            IPEndPoint input;
            udp_client.Receive(ref input);
        }

        private void Guess()
        {
            Console.WriteLine("In Guess Function");
            Console.WriteLine("Input Guess: ");
            string my_guess = Console.ReadLine();
            short id = 1;

            Message message = new Message()
            {
                MessageType = 3,
                game_id = id,
                guess = my_guess
            };
            byte[] bytes = message.Encode();

            foreach (IPEndPoint ep in Peers)
            {
                int bytesSent = udp_client.Send(bytes, bytes.Length, ep);
            }
        }

        private void Hint()
        {
           
            Console.WriteLine("In GetHint Function");
            short id = 1;
            Message message = new Message()
            {
                MessageType = 5,
                game_id = id,
            };
            byte[] bytes = message.Encode();

            foreach (IPEndPoint ep in Peers)
            {
                int bytesSent = udp_client.Send(bytes, bytes.Length, ep);
            }
        }

        private void Exit()
        {
            short id = 1;
            Message message = new Message()
            {
                MessageType = 7,
                game_id = id,
            };
            byte[] bytes = message.Encode();

            foreach (IPEndPoint ep in Peers)
            {
                int bytesSent = udp_client.Send(bytes, bytes.Length, ep);
            }
        }

        private void Ack()
        {
            short id = 1;
            Message message = new Message()
            {
                MessageType = 8,
                game_id = id,
            };
            byte[] bytes = message.Encode();

            foreach (IPEndPoint ep in Peers)
            {
                int bytesSent = udp_client.Send(bytes, bytes.Length, ep);
            }
        }
    }
}