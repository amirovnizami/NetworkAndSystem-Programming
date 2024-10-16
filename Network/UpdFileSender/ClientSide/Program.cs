using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;
Console.WriteLine("Client");

var client = new UdpClient();

var endPoint = new IPEndPoint(IPAddress.Loopback, 27001);

Timer timer = new Timer(5000);
timer.AutoReset = true;
timer.Enabled = true;

while (true)
{
    var bytes = takeScreen();
    byte[] screenshotBytes = takeScreen();
    const int packetSize = 1024;
    //Paketlerin sayiini tapmaq
    int totalPackets = (int)Math.Ceiling((double)screenshotBytes.Length / packetSize);

    for (int i = 0; i < totalPackets; i++)
    {
        int offset = i * packetSize;
        int size = Math.Min(packetSize, screenshotBytes.Length - offset);

        byte[] packet = new byte[size];
        Array.Copy(screenshotBytes, offset, packet, 0, size);

        client.Send(packet, packet.Length, endPoint);
        Console.WriteLine($"Sent packet {i + 1}/{totalPackets}");
    }
    byte[] endSignal = Encoding.UTF8.GetBytes("END::");
    client.Send(endSignal, endSignal.Length, endPoint);

    Console.WriteLine("Screenshot sent! Press 'Enter' to send another:");
    Console.ReadLine();


}

byte[] takeScreen()
{
    Bitmap memoryImage;
    memoryImage = new Bitmap(1920, 1080);
    Size s = new Size(memoryImage.Width, memoryImage.Height);

    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
    memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

    using (MemoryStream ms = new MemoryStream())
    {
        memoryImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }
}