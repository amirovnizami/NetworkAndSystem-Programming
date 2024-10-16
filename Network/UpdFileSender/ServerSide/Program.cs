using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Server");

var server = new UdpClient(27001);
var remoteEp = new IPEndPoint(IPAddress.Any, 0);

while (true)
{
    Console.WriteLine("Waiting for new screenshot...");
    var path = $@"C:\Users\amiro\Desktop\New folder\{Guid.NewGuid()}.png";

    using (var fsWrite = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
        while (true)
        {
            byte[] receivedBytes = server.Receive(ref remoteEp);
            //End signal gelende bitir.
            string message = Encoding.UTF8.GetString(receivedBytes);
            if (message == "END::")
            {
                Console.WriteLine("Screenshot received successfully.");
                break;
            }

            fsWrite.Write(receivedBytes, 0, receivedBytes.Length);
        }

        fsWrite.Close();
    }
}
