using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

var port = 27001;
var ipA = IPAddress.Parse("192.168.100.8");
var endPoint = new IPEndPoint(ipA, port);
var client = new TcpClient();

try
{
    Console.WriteLine("Servere baglanilir...");
    await client.ConnectAsync(endPoint);

    if (client.Connected)
    {
        var sw = new StreamWriter(client.GetStream()) { AutoFlush = true };
        var sr = new StreamReader(client.GetStream());

        Console.WriteLine("'X - 0' oyununa xoş gəldiniz!");
        Console.WriteLine("1. Yeni oyuna basla");
        Console.WriteLine("2. Dostunla oyna");
        Console.WriteLine("2. Cixis");
        Console.Write("Seicm: ");
        var secim = Console.ReadLine();

        if (secim == "1")
        {

            Console.Write("Kiminle oynamaq isteyirsiz? :");

            var remoteEndPoint = Console.ReadLine();
            await sw.WriteLineAsync(remoteEndPoint);
            while (true)
            {
                var response = await sr.ReadLineAsync();
                Console.WriteLine(response);
            }

        }
        else if (secim == "2")
        {

            Console.WriteLine("Serverde gozlenilir...");
            while (true)
            {
                var receiveMsg = await sr.ReadLineAsync();
                if (receiveMsg != null)
                {
                    Console.WriteLine($"{receiveMsg}");
                }
                else Console.WriteLine("Null");

                Console.Write("Oynamaq ucun 'H' eks halda 'Y' a basin: ");
                var chc = Console.ReadLine();

                await sw.WriteLineAsync(chc);

            }
        }

        //gameDisplay();


        //_ = Task.Run(async () =>
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            var receiveMsg = await sr.ReadLineAsync();
        //            if (receiveMsg != null)
        //            {
        //                Console.WriteLine($"{receiveMsg}");
        //            }
        //            else Console.WriteLine("Null");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Baglanti kesildi: {ex.Message}");
        //    }
        //    Task.Delay(5000);
        //});




    }
}
catch (Exception ex)
{
    Console.WriteLine($"Xeta: {ex.Message}");
}

void gameDisplay()
{
    char[,,] array3D =
    {
        {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' }
        }
    };

    array3D[0, 0, 1] = 'a';
    array3D[0, 0, 2] = 'b';
    array3D[0, 0, 0] = 'c';

    for (int layer = 0; layer < array3D.GetLength(0); layer++)
    {
        for (int row = 0; row < array3D.GetLength(1); row++)
        {
            for (int col = 0; col < array3D.GetLength(2); col++)
            {
                Console.Write(" | " + array3D[layer, row, col]);
            }
            Console.WriteLine();
            Console.WriteLine(" ------------");
        }
    }
}


void startGame()
{
    Console.WriteLine("Oyun basladilir...");

    gameDisplay();
}