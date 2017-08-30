using System;

namespace SimpleUDPSocket
{
    public class Program
    {
        public static void Main()
        {
            Console.Write("Do you want to send or receive message? (enter S or R): ");
            string choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice) || choice.Length <= 0)
                return;

            switch (choice.Trim().ToUpper().Substring(0,1))
            {
                case "S":
                    SimpleSender sender = new SimpleSender();
                    sender.DoStuff();
                    break;
                case "R":
                    SimpleReceiver receiver = new SimpleReceiver();
                    receiver.DoStuff();
                    break;
            }
        }
    }
}
