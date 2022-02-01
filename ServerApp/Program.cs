using ServerApp.DataAccess;
using System;

namespace ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            var ctx = new UsersRedisContext();
            foreach (var item in ctx.Users)
            {
                Console.WriteLine(item.Username + ":" + item.Likes);
            }
        }
    }
}
