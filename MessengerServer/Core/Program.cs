using MessengerServer.RequestHandlers;
using MessengerServer.Services;
using MessengerShared;
using System.Net;

namespace MessengerServer.Core
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "ServerLogs";
            IPAddress ip = IPAddress.Any;
            Server server = new Server(ip, Protocol.DefaultPort, true);
            server.Run();
            while(true)
                Console.ReadKey();
        }
    }
}