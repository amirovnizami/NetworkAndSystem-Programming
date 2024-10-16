using System.Diagnostics.Metrics;
using System;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HttpServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml;

HttpListener listener = new HttpListener();
var port = 27001;
listener.Prefixes.Add($"http://localhost:{port}/");
listener.Start();

Console.WriteLine($"Listening on {port}");

while (true)
{
    var context = listener.GetContext();//Portu dinleyir
    var request = context.Request;//Http request  -- Clientden gelen requeste qarsiliq gelir
    var response = context.Response;// Http cliente response qaytarirq ve client goturur bu responsu

    if (request.HttpMethod == "GET")
    {
        string responseString = HandleGetAllUsers();
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);

        //responsu cliente gondermek byte byte
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }

    if (request.HttpMethod == "POST")
    {
        //Clientden geler contenti streamreader ile oxuma
        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        {
            var json = reader.ReadToEnd();
            var user = JsonSerializer.Deserialize<User>(json);
            if (user != null)
            {

                using (var DbContext = new UsersDbContext())
                {
                    DbContext.Users.Add(user);
                    DbContext.SaveChanges();
                }
            }
            else Console.WriteLine("User can't receieved!");


        }
        string responseMessagge = "User added successfuly!";

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseMessagge);

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message); ;
        }

    }

    if (request.HttpMethod == "DELETE")
    {
        var slpitSegments = request.Url.AbsolutePath.Split('/');

        var deleteId = Int32.Parse(slpitSegments[1]);

        using (var dbContext = new UsersDbContext())
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == deleteId);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
            else Console.WriteLine("User not found!");
        }
        var responseMessagge = "User deleted successfuly!";

        byte[] buffer = Encoding.UTF8.GetBytes(responseMessagge);

        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}


string HandleGetAllUsers()
{
    try
    {
        using (var Dbcontext = new UsersDbContext())
        {
            var users = Dbcontext.Users.ToListAsync();
            return JsonSerializer.Serialize(users);
        }
    }
    catch (Exception ex)
    {

        return ($"Error:{ex.Message}");
    }
}