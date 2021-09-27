using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Common;
using Newtonsoft.Json;

namespace Broker
{
    public class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            if (payloadString.StartsWith("subscribe#"))
            {
                connectionInfo.Topic = payloadString.Split("subscribe#").LastOrDefault();
                ConnectionStorage.Add(connectionInfo);
            }
            else
            {
                Payload payload = JsonConvert.DeserializeObject<Payload>(payloadString);
                PayloadStorage.Add(payload);
            }
            
            Console.WriteLine(payloadString);
        }
    }
}