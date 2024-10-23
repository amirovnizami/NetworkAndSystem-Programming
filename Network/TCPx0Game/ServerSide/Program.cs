using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var port = 27001;
var ipAd = IPAddress.Parse("192.168.100.8");
var clients = new List<TcpClient>();
var endPoint = new IPEndPoint(ipAd, port);

Console.WriteLine("Server started...");

var listener = new TcpListener(endPoint);
Console.WriteLine("Onlayn olan oyuncular : ");
listener.Start();

try
{
    while (true)
    {

        var client = await listener.AcceptTcpClientAsync();
        clients.Add(client);
        _ = Task.Run(async () =>
        {

            try
            {
                Console.WriteLine($"{client.Client.RemoteEndPoint}");

                var stream = client.GetStream();
                var sr = new StreamReader(stream);
                var sw = new StreamWriter(stream) { AutoFlush = true };

                while (true)
                {
                    var target_Client = await sr.ReadLineAsync();
                    if (string.IsNullOrEmpty(target_Client))
                    {
                        Console.WriteLine("Istek yoxdur");
                        continue;
                    }
                    Console.WriteLine($"Mesaj:  {target_Client}");

                    var targetClient = clients.Find(c => c.Client.RemoteEndPoint.ToString() == target_Client);
                    if (targetClient != null)
                    {
                        var sw_TargetClient = new StreamWriter(targetClient.GetStream()) { AutoFlush = true };
                        var sr_TargetClient = new StreamReader(targetClient.GetStream());

                        await sw_TargetClient.WriteLineAsync($"{target_Client} : sizinle oyun oynamaq isteyir razisinizmi? :");
                        var msj = await sr_TargetClient.ReadLineAsync();

                        if (msj == "H")
                        {
                            await sw.WriteLineAsync("Oyun qebul edildi.Oyun basliyir..");
                            await sw_TargetClient.WriteLineAsync("Oyun qebul etdiniz oyun basliyir...");
                            Console.WriteLine("Cavan gonderildi");
                        }
                        else if (msj == "Y")
                        {
                            await sw.WriteLineAsync("Oyun redd edildi.");
                            await sw_TargetClient.WriteLineAsync("Siz oyunu redd elediz.");
                            Console.WriteLine("Cavab gonderildi");
                        }


                    }
                    else
                    {
                        Console.WriteLine("Bele oyuncu tapilmadi.");
                    }


                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }



        });

    }
}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}

