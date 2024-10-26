using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

var port = 27001;
var ipA = IPAddress.Parse("192.168.100.8");
var endPoint = new IPEndPoint(ipA, port);
var client = new TcpClient();

try
{
    Console.WriteLine("Connecting to Server...");
    await client.ConnectAsync(endPoint);

    if (client.Connected)
    {
        var sw = new StreamWriter(client.GetStream()) { AutoFlush = true };
        var sr = new StreamReader(client.GetStream());

        Console.WriteLine("-------'X - 0' Welcome------");
        Console.WriteLine("1. New Game");
        Console.WriteLine("2. With Friends");
        Console.WriteLine("3. Exit");
        Console.Write("Seçim: ");
        var secim = Console.ReadLine();

        if (secim == "1")
        {
            Console.Write("Who would you like to play with? (IP:Port): ");
            var remoteEndPoint = Console.ReadLine()?.Trim();

            await sw.WriteLineAsync(remoteEndPoint);

            while (true)
            {
                var response = await sr.ReadLineAsync();
                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine(response);
                    if (response.Contains("Starting game"))
                    {
                        GameDisplay2();
                        StartGame();
                        //break;
                    }
                }
                else
                {
                    Console.WriteLine("Connection closed by the server.");
                    break;
                }
            }
        }
        else if (secim == "2")
        {
            Console.WriteLine("Waiting for a friend to connect...");

            while (true)
            {
                var receiveMsg = await sr.ReadLineAsync();
                if (!string.IsNullOrEmpty(receiveMsg))
                {
                    Console.WriteLine($"{receiveMsg}");
                    GameDisplay2();
                    var list = GameDisplay();
                    var json = JsonSerializer.Serialize(list);
                    sw.WriteLine(json);
                }
                else
                {
                    Console.WriteLine("Connection closed.");
                    break;
                }
            }
        }
        else if (secim == "3")
        {
            Console.WriteLine("Exiting the game.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

void StartGame()
{
    Console.WriteLine("Game has started! Here's the board:");
    GameDisplay();
}

string[] GameDisplay()
{
    char[,,] board =
    {
        {
            { '1', '2', '3' },
            { '4', '5', '6' },
            { '7', '8', '9' }
        }
    };

    Console.Write("Make your move (e.g., 1-X): ");
    var mv = Console.ReadLine();

    var moeList = mv.Split('-'); 
    var index = moeList[0];
    var chr = moeList[1];

    if (index == "1") board[0, 0, 0] = char.Parse(chr);
    else if (index == "2") board[0, 0, 1] = char.Parse(chr);
    else if (index == "3") board[0, 0, 2] = char.Parse(chr);
    else if (index == "4") board[0, 1, 0] = char.Parse(chr);
    else if (index == "5") board[0, 1, 1] = char.Parse(chr);
    else if (index == "6") board[0, 1, 2] = char.Parse(chr);
    else if (index == "7") board[0, 2, 0] = char.Parse(chr);
    else if (index == "8") board[0, 2, 1] = char.Parse(chr);
    else if (index == "9") board[0, 2, 2] = char.Parse(chr);

    for (int row = 0; row < board.GetLength(1); row++)
    {
        for (int col = 0; col < board.GetLength(2); col++)
        {
            Console.Write($" | {board[0, row, col]}");
        }
        Console.WriteLine();
        Console.WriteLine(" ------------");
    }

    return moeList;
}
void GameDisplay2()
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