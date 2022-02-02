using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace CacheServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var usersDict = new Dictionary<string, int>();
                var searchUsersDict = new Dictionary<string, int>();

                Console.WriteLine(">>> Cache server app <<<");
                Console.WriteLine("Listener...");
                var ipAddress = IPAddress.Parse("127.0.0.1");
                var cacheServer = new TcpListener(ipAddress, 45678);
                cacheServer.Start(100);

                while (true)
                {
                    var client = cacheServer.AcceptTcpClient();
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected.");

                    var stream = client.GetStream();
                    var br = new BinaryReader(stream);
                    var bw = new BinaryWriter(stream);

                    Task.Run(() =>
                    {
                        while (true)
                        {
                            var username = br.ReadString();
                            Console.WriteLine($"{client.Client.RemoteEndPoint} terefinden \"{username}\" search olundu!");

                            if (usersDict.ContainsKey(username))
                            {
                                bw.Write(true);
                                bw.Write(usersDict.GetValueOrDefault(username));
                                var input = br.ReadString();
                                var command = JsonSerializer.Deserialize<Command>(input);
                                Console.WriteLine("Command: " + command.Text + " " + command.Param);
                                Console.WriteLine($"\"{username}\" ile bagli melumat cache datada tapildi!");

                            }
                            else
                            {
                                bw.Write(false);
                                var input = br.ReadString();
                                var command = JsonSerializer.Deserialize<Command>(input);
                                Console.WriteLine("Command: " + command.Text + " " + command.Param);
                                Console.WriteLine($"\"{username}\" ile bagli melumat cache datada tapilmadi!");
                                var likesUsername = br.ReadInt32();
                                if (!searchUsersDict.ContainsKey(username))
                                {
                                    searchUsersDict.Add(username, 1);
                                }
                                else
                                {
                                    if (searchUsersDict[username] < 3)
                                    {
                                        searchUsersDict[username] += 1;
                                        if (searchUsersDict[username] == 3)
                                        {
                                            usersDict.Add(username, likesUsername);
                                            Console.WriteLine($"\"{username}\" cache dataya elave olundu!");
                                        }
                                    }
                                }
                            }

                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class Command
        {
            public const string Get = "GET";
            public const string Put = "PUT";

            public string Text { get; set; }
            public string Param { get; set; }
        }

    }
}
