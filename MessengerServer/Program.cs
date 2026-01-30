using System.Net;

namespace MessengerServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "ServerLogs";
            int port = 5000;
            IPAddress ip = IPAddress.Any;
            Server server = new Server(ip,port);
            /*new Thread(() =>
            {
                Console.ReadLine();
                server.m_running = false;
            }).Start();*/
            server.Run();
        }
    }
}