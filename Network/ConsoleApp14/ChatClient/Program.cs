using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var server = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            var ipAddress = IPAddress.Parse("192.168.100.8");

            var port = 57537;

            var endPoint = new IPEndPoint(ipAddress, port);

            try
            {
                server.Connect(endPoint);

                if (server.Connected)
                {

                    while (true)
                    {
                        var msg = Console.ReadLine();
                        var bytes = Encoding.UTF8.GetBytes(msg);
                        server.Send(bytes);
                    }
                }
                else Console.WriteLine("Cannot connect");

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
