using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

var port = 27001;
var ipAddress = IPAddress.Parse("192.168.100.8");
var endPoint = new IPEndPoint(ipAddress, port);

var clients = new List<TcpClient>();
TcpListener listener = new TcpListener(endPoint);

Console.WriteLine("Server starting...");
listener.Start();
TcpClient? targetClient = null;

try
{
    while (true)
    {
        var client = await listener.AcceptTcpClientAsync();
        clients.Add(client);
        Console.WriteLine($"New client connected: {client.Client.RemoteEndPoint}");

        _ = Task.Run(async () =>
        {
            try
            {
                var stream = client.GetStream();
                var sr = new StreamReader(stream);
                var sw = new StreamWriter(stream) { AutoFlush = true };

                while (true)
                {
                    var request = await sr.ReadLineAsync();
                    if (string.IsNullOrEmpty(request))
                    {
                        Console.WriteLine("Client disconnected.");
                        break;
                    }

                    Console.WriteLine($"Received: {request}");

                    if (IsRemoteEndpoint(request))
                    {
                        targetClient = clients.Find(c =>
                            c.Client.RemoteEndPoint?.ToString() == request.Trim());

                        if (targetClient != null)
                        {
                            await sw.WriteLineAsync("Player found! Starting game...");
                            await StartGame(targetClient);
                        }
                        else
                        {
                            await sw.WriteLineAsync("Player not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Waiting for player input...");
                        var response = await sr.ReadLineAsync();
                        Console.WriteLine($"Player response: {response}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Server error: {ex.Message}");
}

async Task StartGame(TcpClient targetClient)
{
    
    var sw = new StreamWriter(targetClient.GetStream()) { AutoFlush = true };
    var sr = new StreamReader(targetClient.GetStream());
    await sw.WriteLineAsync("Game started! Make your move.");
    var json = await sr.ReadLineAsync();
    string[] moveList = JsonSerializer.Deserialize<string[]>(json);

    foreach(var move in moveList)
    {
        Console.WriteLine(move);
    }
    GameDisplay();
}

void GameDisplay()
{
    char[,,] board =
    {
        {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' }
        }
    };

    
    Console.WriteLine("Game Board:");
    for (int row = 0; row < board.GetLength(1); row++)
    {
        for (int col = 0; col < board.GetLength(2); col++)
        {
            Console.Write($" | {board[0, row, col]}");
        }
        Console.WriteLine();
        Console.WriteLine(" ------------");
    }
}

static bool IsRemoteEndpoint(string input)
{
    string pattern = @"^(\d{1,3}\.){3}\d{1,3}:\d+$";
    return Regex.IsMatch(input, pattern);
}
