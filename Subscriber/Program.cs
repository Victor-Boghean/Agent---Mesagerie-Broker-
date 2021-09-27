using System;
using Common;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Subscriber");

            string topic;
            
            Console.Write("Enter the ropic: ");
            topic = Console.ReadLine().ToLower();

            var SubscriberSocker = new SubscriberSocket(topic);
            SubscriberSocker.Connect(Settings.BROKER_IP, Settings.BROKER_PORT);
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}