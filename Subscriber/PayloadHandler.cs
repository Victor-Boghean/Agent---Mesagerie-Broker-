using System;
using System.Text;
using Common;
using Newtonsoft.Json;

namespace Subscriber
{
    public class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<Payload>(payloadString);
            
            Console.WriteLine(payload.Message);
        }
    }
}