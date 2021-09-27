using System;
using System.Net;
using System.Net.Sockets;
using Common;

namespace Broker
{
    public class BrokerSocket
    {
        private Socket _soket;
        private const int CONNECTIONS_LIMIT = 8;
        
        public BrokerSocket()
        {
            _soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            _soket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            _soket.Listen(CONNECTIONS_LIMIT);
            Accept();
        }

        private void Accept()
        {
            _soket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.Socket = _soket.EndAccept(asyncResult);
                connection.Address = connection.Socket.RemoteEndPoint.ToString();
                connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length,
                    SocketFlags.None, ReceiveCallback, connection);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't accept. {e.Message}");
            }
            finally
            {
                Accept();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                Socket senderSocket = connection.Socket;
                SocketError response;
                int buffSize = senderSocket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payload = new byte[buffSize];
                    Array.Copy(connection.Data, payload, payload.Length);
                    PayloadHandler.Handle(payload, connection);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't receive data. {e.Message}");
            }
            finally
            {
                try
                {
                    connection.Socket.BeginReceive(connection.Data, 0, connection.Data.Length, 
                        SocketFlags.None, ReceiveCallback, connection);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    var address = connection.Socket.RemoteEndPoint.ToString();
                    
                    ConnectionStorage.Remove(address);
                    connection.Socket.Close();
                }
            }
        }
    }
}