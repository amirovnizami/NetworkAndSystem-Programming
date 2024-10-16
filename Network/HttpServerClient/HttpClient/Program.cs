
using HttpServer.Models;
using System.Text;
using System.Text.Json;


var port = 27001;

HttpClient client = new HttpClient();

while (true)
{
    Console.WriteLine("---------------------------------------");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("1.All users: ");
    Console.WriteLine("2.Add user: ");
    Console.WriteLine("3.Delete user: ");
    Console.WriteLine("Press 'x' to exit: ");

    Console.Write("Choice : ");
    var choice = Console.ReadLine();
    if (choice == "1")
    {
        HttpResponseMessage response = await client.GetAsync("http://localhost:27001");

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            string[] users = responseBody.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }
        else Console.WriteLine("Error");
    }
    else if (choice == "2")
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Age: ");
        var age = int.Parse(Console.ReadLine()!);

        var user = new User
        {
            Name = name!,
            Email = email!,
            Age = age,
            RegistrationDate = DateTime.Now
        };
        var json = JsonSerializer.Serialize(user);

        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("http://localhost:27001/users", content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
    }
    else if (choice == "3")
    {
        Console.Write("Enter id(will be deleted) : ");
        var deleteId = Console.ReadLine()!;

        HttpResponseMessage response = await client.DeleteAsync($"http://localhost:27001/{deleteId}");

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }


    }
    else if(choice == "x")
    {
        break;
    }
    else Console.WriteLine("Incorrect choice");
}
