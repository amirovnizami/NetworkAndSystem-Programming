using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAdress = IPAddress.Parse("192.168.100.8");
            var port = 57537;

            var endPoint = new IPEndPoint(ipAdress,port);

            listener.Bind(endPoint);
            listener.Listen();

            while (true)
            {
                var client = listener.Accept();
                Console.WriteLine("SERVER :");
                Console.WriteLine($"{client.RemoteEndPoint} is connected");

                Task.Run(() =>
                {

                    var bytes = new byte[1024];

                    var msg = string.Empty;
                    var len = 0;
                    while (true)
                    {
                        len = client.Receive(bytes);
                        msg = Encoding.UTF8.GetString(bytes, 0, len);

                        if (msg.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                        {
                            client.Shutdown(SocketShutdown.Send);
                            client.Close();
                            break;
                        }
                        Console.WriteLine($"{client.RemoteEndPoint} {msg}");
                    }

                });
            }
        }
    }
}
