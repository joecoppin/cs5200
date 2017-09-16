using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using log4net;
using log4net.Config;

namespace Server_Client
{
    class Player
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Player));

        private readonly UdpClient udp_client;
        IPEndPoint endPoint;
        
        private short messageType = 0, id = 0, score = 0;
        private string my_a_num = "", my_l_name = "", my_f_name = "", my_alias = "", hint = "", def = "", error = "", my_server_info = "";
        private byte result = 0;


        public Player()
        {
            IPEndPoint local_endpoint = new IPEndPoint(IPAddress.Any, 0);
            udp_client = new UdpClient(local_endpoint);
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine($"(N)ew Game | (H)int | (G)uess | (Q)uit | (U)pdate Info \n");
            Console.WriteLine($"Score {score} \t\t{my_f_name} {my_alias} {my_a_num}\n\n");
            Console.WriteLine($"Definition: {def}");
            Console.WriteLine($"Answer: {hint} \n\n");
            Console.WriteLine($"Server Info {my_server_info} \t ");
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
                    case "U":
                        UpdateInfo();
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
                            logger.Debug("GameDef Message Received");
                            logger.Debug($"Game ID: {message.game_id}");
                            logger.Debug($"Defition: {message.def}");
                            logger.Debug($"Hint: {message.hint}");
                            def = message.def;
                            hint = message.hint;
                            Display();
                        }
                        if (message.MessageType == 4)
                        {
                            logger.Debug("Answer Message Received");
                            logger.Debug($"Game ID: {message.game_id}");
                            logger.Debug($"Result: {message.result}");
                            logger.Debug($"Score: {message.score}");
                            logger.Debug($"Hint: {message.hint}");
                            score = message.score;
                            result = message.result;
                            hint = message.hint;
                            if (result == 1)
                            {
                                Display();
                                Console.WriteLine("\n\nCorrect!");
                            }
                            else
                            {
                                Display();
                                Console.WriteLine("\n\nIncorrect");
                            }  
                        }
                        if (message.MessageType == 6)
                        {
                            logger.Debug("Hint Message Received");
                            logger.Debug($"Game ID: {message.game_id}");
                            logger.Debug($"Hint: {message.hint}");
                            hint = message.hint;
                            Display();
                        }
                        if (message.MessageType == 9)
                        {
                            logger.Debug("Error Message Received");
                            logger.Debug($"Game ID: {message.game_id}");
                            logger.Debug($"Error: {message.error}");
                            error = message.error;
                            Display();
                        }
                        if (message.MessageType == 10)
                        {
                            logger.Debug("Heartbeat Message Received");
                            logger.Debug($"Game ID: {message.game_id}");
                            Ack();
                        }

                    }
                }
            }
        }

        public void Run(string serverPortAddress, List<string> info)
        {
            bool run = true;
            string address = serverPortAddress;
            my_server_info = serverPortAddress;
            List<string> myList = info;
            my_f_name = myList.ElementAt(0);
            my_l_name = myList.ElementAt(1);
            my_a_num = myList.ElementAt(2);
            my_alias = myList.ElementAt(3);

            IPEndPoint myEndPoint = Endpoints.Parse(address);
            endPoint = myEndPoint;
            GetUserInput();
            Thread handle = new Thread(new ThreadStart(HandleIncommingCom));
            handle.Start();
            while (run)
            {
                GetUserInput();
            }
            handle.Join();
        }

        private void NewGame()
        {
            logger.Info("In NewGame Function--Creating and Sending message");
        // Console.WriteLine("In NewGame Function");
        Message message = new Message()
            {
                MessageType = 1,
                a_num = my_a_num,
                l_name = my_l_name,
                f_name = my_f_name,
                alias = my_alias
            };

            byte[] bytes = message.Encode_NewGame();
            logger.Debug(bytes);
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }

        private void Guess()
        {
            logger.Info("In Guess Function");
            Console.WriteLine("Input Guess: ");
            string my_guess = Console.ReadLine();

            Message message = new Message()
            {
                MessageType = 3,
                game_id = id,
                guess = my_guess
            };
            byte[] bytes = message.Encode_Guess();
            logger.Debug(bytes);
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }

        private void Hint()
        {
            logger.Info("In GetHint Function");
            Message message = new Message()
            {
                MessageType = 5,
                game_id = id,
            };
            byte[] bytes = message.Encode_Hint_Exit_Ack();
            logger.Debug(bytes);
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
            logger.Debug(bytes);
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
            SaveSettings();
            Environment.Exit(0);
        }

        private void Ack()
        {

            Message message = new Message()
            {
                MessageType = 8,
                game_id = id,
            };
            byte[] bytes = message.Encode_Hint_Exit_Ack();
            logger.Debug(bytes);
            int bytesSent = udp_client.Send(bytes, bytes.Length, endPoint);
        }
        public void UpdateInfo()
        {
            Console.WriteLine("Please enter your First Name: ");
            string first = Console.ReadLine();
            Console.WriteLine("Please enter your Last Name: ");
            string last = Console.ReadLine();
            Console.WriteLine("Please enter your A#: ");
            string aNum = Console.ReadLine();
            Console.WriteLine("Please enter your Alias: ");
            string alias = Console.ReadLine();

            my_f_name = first;
            my_l_name = last;
            my_a_num = aNum;
            my_alias = alias;

            Display();
        }

        void SaveSettings()
        {
            string personal = my_f_name + " " + my_l_name + " " + my_a_num + " " + my_alias;
            string server = my_server_info;

            System.IO.File.WriteAllText("user.txt", personal);
            System.IO.File.WriteAllText("server.txt", server);                   
        }
    }

     class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        public static string GetServer()
        {
            Console.WriteLine("Please input server and port number: (Ex: 127.0.0.1:12001)");
            string server = Console.ReadLine();
            return server;
        }
        
        public static List<string> ReadFile(string file)
        {
            var list = new List<string>();
            string[] words;
            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                words = streamReader.ReadToEnd().Split(' ');
            }

            //add words to list
            list.AddRange(words);

            return list;
        }
        
        public static List<string> GetInfo()
        {
            List<string> info = new List<string>();
            Console.WriteLine("Please enter your First Name: ");
            string first = Console.ReadLine();
            Console.WriteLine("Please enter your Last Name: ");
            string last = Console.ReadLine();
            Console.WriteLine("Please enter your A#: ");
            string aNum = Console.ReadLine();
            Console.WriteLine("Please enter your Alias: ");
            string alias = Console.ReadLine();
            info.Add(first);
            info.Add(last);
            info.Add(aNum);
            info.Add(alias);

            return info;
        }
        static void Main()
        {

            XmlConfigurator.Configure();
            string serverPortAddress = "", input = "", user_f = "user.txt", server_f = "server.txt";
            List<string> info = new List<string>();
                       
            if (File.Exists(user_f))
            {
                Console.WriteLine("A User Config File exists.  Would you like to use the default user information? (y/n)");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input) && input.Length > 0)
                {
                    switch (input.Trim().ToUpper().Substring(0, 1))
                    {
                        case "N":
                            info = GetInfo();
                            break;
                        case "Y":
                            info = ReadFile(user_f);
                            break;
                    }
                }
            }
            if(!File.Exists(user_f))
            {
                info = GetInfo();
            }
            if (File.Exists(server_f))
            {
                Console.WriteLine("Would you like to use the server address and port used in the last session? (y/n)");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input) && input.Length > 0)
                {
                    switch (input.Trim().ToUpper().Substring(0, 1))
                    {
                        case "N":
                            serverPortAddress = GetServer();
                            logger.Info($"Server and Port Number: {serverPortAddress}");
                            break;
                        case "Y":
                            List<string> serverInfo =ReadFile(server_f);
                            serverPortAddress = serverInfo.First();
                            break;
                    }
                }
            }
            if (!File.Exists(server_f))
            {
                serverPortAddress = GetServer();
                logger.Info($"Server and Port Number: {serverPortAddress}");
            }
            

            Player player = new Player();
            player.Run(serverPortAddress, info);
        }
    }
}