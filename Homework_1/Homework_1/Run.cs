using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Server_Client
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Input IP Address and Port Number: (Ex: 127.0.0.1:8000)");
            string address = Console.ReadLine();
            Console.Write("Enter whether Server or Client (S or C): ");
            string choice = Console.ReadLine();
            if (!string.IsNullOrEmpty(choice) && choice.Length > 0)
            {
                switch (choice.Trim().ToUpper().Substring(0, 1))
                {
                    case "C":
                        Player player = new Player();
                        player.Run();
                        break;
                    case "S":
                       Server server = new Server();
                        server.Run();
                        break;
                }
            }
        }
    }
}
