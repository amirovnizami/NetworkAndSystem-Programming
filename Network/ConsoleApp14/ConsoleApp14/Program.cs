using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp14
{
    internal class Program
    {
        static void Main(string[] args)
        {
           var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipAdress = IPAddress.Parse("142.250.184.142");

            var port = 80;

            var endPoint = new IPEndPoint(ipAdress, port);

            try
            {
                socket.Connect(endPoint);
                if (socket.Connected)
                {
                    var str = "GET\r\n\r\n";
                    var bytes = Encoding.ASCII.GetBytes(str);
                    socket.Send(bytes);

                    var length = 0;
                    var buffer = new byte[1024];

                    do
                    {
                        length = socket.Receive(buffer);
                        var response = Encoding.ASCII.GetString(buffer);
                        Console.WriteLine(response);
                        Thread.Sleep(200);
                    } while (length > 0);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }

          
        }
    }
}
