using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class Program
{
    static void Main()
    {
        var port = 27001;
        var IpAd = IPAddress.Parse("192.168.100.8");
        var endPoint = new IPEndPoint(IpAd, port);

        var client = new TcpClient();

        try
        {
            client.Connect(endPoint);
            if (client.Connected)
            {
                Console.WriteLine("Connected to server.");
                var path = @"C:\Users\amiro\Downloads\cinema1.jpg";
                var networkStream = client.GetStream();

                //Console.Write("Enter path : ");
                //var path = Console.ReadLine();
                using (var fRead = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[1024];
                    var len = 0;
                    while ((len = fRead.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        networkStream.Write(buffer, 0, len);
                    }
                }


                Console.WriteLine("File sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}
