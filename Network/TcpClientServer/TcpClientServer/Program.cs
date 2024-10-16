using Microsoft.VisualBasic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

var port = 27001;

var ipAd = IPAddress.Parse("192.168.100.8");

var endPoint = new IPEndPoint(ipAd, port);

var listener = new TcpListener(endPoint);

try
{
    listener.Start();

    while (true)
    {
        var client = listener.AcceptTcpClient();

        _ = Task.Run(() =>
        {
            Console.WriteLine($"{client.Client.RemoteEndPoint} connected");
            var stream = client.GetStream();
            var path = "img_copy.jpg";

            using (var fWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                int len = 0;
                var bytes = new byte[1024];
                while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    fWrite.Write(bytes, 0, len);
                }
                Console.WriteLine("File receieved!");
                client.Close();
            }
            
        });
    }
}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}