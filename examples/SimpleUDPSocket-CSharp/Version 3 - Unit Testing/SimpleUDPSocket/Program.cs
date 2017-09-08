﻿using System;

using log4net;
using log4net.Config;

namespace SimpleUDPSocket
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            XmlConfigurator.Configure();
            Logger.Info("Starting up");

            Console.Write("Do you want to send or receive message? (enter S or R): ");
            string choice = Console.ReadLine();
            if (!string.IsNullOrEmpty(choice) && choice.Length>0)
            {
                switch (choice.Trim().ToUpper().Substring(0,1))
                {
                    case "S":
                        SimpleSender sender = new SimpleSender();
                        sender.SendStuff();
                        break;
                    case "R":
                        SimpleReceiver receiver = new SimpleReceiver();
                        receiver.ReceiveStuff();
                        break;                  
                }
            }
        }

    }
}
