using ServerApp.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var ctx = new UsersRedisContext();
                Console.WriteLine(">>> Server app <<<");
                Console.WriteLine("Listener...");
                var ipAddress = IPAddress.Parse("127.0.0.1");
                var listener = new TcpListener(ipAddress, 45001);
                listener.Start(100);

                while (true)
                {
                    // Main server connected
                    var client = listener.AcceptTcpClient();
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected.");

                    var stream = client.GetStream();
                    var br = new BinaryReader(stream);
                    var bw = new BinaryWriter(stream);

                    // Cache server connected
                    TcpClient cache = new TcpClient();
                    cache.Connect(ipAddress, 45678);

                    var streamCache = cache.GetStream();
                    var brCache = new BinaryReader(streamCache);
                    var bwCache = new BinaryWriter(streamCache);


                    Task.Run(() =>
                    {
                        while (true)
                        {

                            var username = br.ReadString();

                            Command cmd = null;
                            var input = username;

                            Console.WriteLine($"{client.Client.RemoteEndPoint} terefinden \"{username}\" search olundu!");
                            string responseMainServer;
                            string responseCacheServer;

                            bwCache.Write(username);
                            var isFoundUser = brCache.ReadBoolean();

                            if (isFoundUser)
                            {
                                input = "PUT " + username;
                                cmd = new Command
                                {
                                    Text = input.Split()[0],
                                    Param = input.Split()[1]
                                };
                                bwCache.Write(JsonSerializer.Serialize(cmd));

                                var likes = brCache.ReadInt32();
                                responseCacheServer = $"{username} --> likes: {likes}";
                                Console.WriteLine($"Melumat {client.Client.RemoteEndPoint}-e cache datadan gonderildi!");
                                bw.Write(responseCacheServer);
                            }
                            else
                            {
                                input = "GET " + username;
                                cmd = new Command
                                {
                                    Text = input.Split()[0],
                                    Param = input.Split()[1]
                                };

                                bwCache.Write(JsonSerializer.Serialize(cmd));

                                var searchUser = ctx.Users.Find(username);

                                if (searchUser != null)
                                {
                                    bwCache.Write((int)searchUser.Likes);
                                    responseMainServer = $"{searchUser.Username} --> likes: {searchUser.Likes}";
                                    Console.WriteLine($"Melumat {client.Client.RemoteEndPoint}-e databazadan gonderildi!");
                                }
                                else
                                {
                                    responseMainServer = $"\"{username}\" tapilmadi!";
                                    Console.WriteLine($"\"{username}\" ile bagli melumat databazada tapilmadi!");
                                }
                                bw.Write(responseMainServer);
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
