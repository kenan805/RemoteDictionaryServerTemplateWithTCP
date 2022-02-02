using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClientApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ReadLine();
                Console.WriteLine(">>> Client app 1 <<<");
                var ipAddress = IPAddress.Parse("127.0.0.1");
                var port = 45001;
                var client = new TcpClient();
                client.Connect(ipAddress, port);

                var stream = client.GetStream();
                var br = new BinaryReader(stream);
                var bw = new BinaryWriter(stream);

                while (true)
                {
                    Console.Write("Search username: ");
                    var searchUsername = Console.ReadLine();
                    bw.Write(searchUsername);

                    var responseFromServer = br.ReadString();
                    Console.WriteLine(responseFromServer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }


}
