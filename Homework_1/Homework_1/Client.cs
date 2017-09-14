
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Server_Client
{
    class Player
    {

        private readonly UdpClient udp_client;
        private static IPAddress tosend = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(tosend, 12001);
        
        private short messageType = 0, id = 0, score = 0;
        private string a_num = "", l_name = "", f_name = "", alias = "", hint = "", def = "", guess = "", error = "";
        private byte result = 0;


        public Player()
        {
            IPEndPoint local_endpoint = new IPEndPoint(IPAddress.Any, 0);
            udp_client = new UdpClient(local_endpoint);
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine($"(N)ew Game | (H)int | (G)uess | (Q)uit \t\t Score {score}");
            Console.WriteLine($"Definition: {def}");
            Console.WriteLine($"Answer: {hint}");

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

        public void HandleIncommingCom(){
            while (true)
            {
                byte[] bytes = null;

                //read socket
                try
                {
                    bytes = udp_client.Receive(ref endPoint);
                }
                catch (SocketException error)
                {
                    if (error.SocketErrorCode != SocketError.TimedOut)
                        throw;
                }
                //decode
                if (bytes != null)
                {
                    Message message = Message.Decode(bytes);
                    if (message != null)
                    {
                        messageType = message.MessageType;
                        id = message.game_id;

                        if (message.MessageType == 2)
                        {
                            //Console.WriteLine("GameDef Message Received");
                            //Console.WriteLine($"Game ID: {message.game_id}");
                            //Console.WriteLine($"Defition: {message.def}");
                            // Console.WriteLine($"Hint: {message.hint}");
                            def = message.def;
                            hint = message.hint;
                        }
                        if (message.MessageType == 4)
                        {
                            //Console.WriteLine("Answer Message Received");
                            // Console.WriteLine($"Game ID: {message.game_id}");
                            // Console.WriteLine($"Result: {message.result}");
                            // Console.WriteLine($"Score: {message.score}");
                            //Console.WriteLine($"Hint: {message.hint}");
                            score = message.score;
                            result = message.result;
                            hint = message.hint;
                        }
                        if (message.MessageType == 6)
                        {
                            //Console.WriteLine("Hint Message Received");
                            // Console.WriteLine($"Game ID: {message.game_id}");
                            //Console.WriteLine($"Hint: {message.hint}");
                            hint = message.hint;
                        }
                        if (message.MessageType == 9)
                        {
                            //Console.WriteLine("Error Message Received");
                            // Console.WriteLine($"Game ID: {message.game_id}");
                            // Console.WriteLine($"Error: {message.error}");
                            error = message.error;
                        }
                        if (message.MessageType == 10)
                        {
                            //Console.WriteLine("Heartbeat Message Received");
                            // Console.WriteLine($"Game ID: {message.game_id}");
                            Ack();
                        }

                    }
                }
                //handle --update variables as needed     
            }
        }

        public void Run()
        {
            bool run = true;
            GetUserInput();
            Thread handle = new Thread(new ThreadStart(HandleIncommingCom));
            //Thread getUser = new Thread(new ThreadStart(GetUserInput));
            handle.Start();
            //getUser.Start();
            while (run)
            {
                GetUserInput();
            }
            handle.Join();
            //getUser.Join();
        }

        private void NewGame()
        {
        // Console.WriteLine("In NewGame Function");
        Message message = new Message()
            {
                MessageType = 1,
                a_num = "a01499868_hardcoded",
                l_name = "coppin_hc",
                f_name = "joe_hc",
                alias = "jc"
            };

            byte[] bytes = message.Encode_NewGame();
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }

        private void Guess()
        {
           // Console.WriteLine("In Guess Function");
            Console.WriteLine("Input Guess: ");
            string my_guess = Console.ReadLine();

            Message message = new Message()
            {
                MessageType = 3,
                game_id = id,
                guess = my_guess
            };
            byte[] bytes = message.Encode_Guess();

            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }

        private void Hint()
        {
            //Console.WriteLine("In GetHint Function");
            Message message = new Message()
            {
                MessageType = 5,
                game_id = id,
            };
            byte[] bytes = message.Encode_Hint_Exit_Ack();
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }

        private void Exit()
        {
            Message message = new Message()
            {
                MessageType = 7,
                game_id = id,
            };
            byte[] bytes = message.Encode_Hint_Exit_Ack();
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }

        private void Ack()
        {

            Message message = new Message()
            {
                MessageType = 8,
                game_id = id,
            };
            byte[] bytes = message.Encode_Hint_Exit_Ack();
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }
    }

     class Program
    {
        static void Main()
        { 
            Player player = new Player();
            player.Run();
        }
    }
}