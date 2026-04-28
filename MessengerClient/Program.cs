using MessengerClient.Client;
using MessengerClient.Client.Protocol;
using MessengerClient.Client.Services;
using MessengerClient.Client.Transport;
using MessengerClient.Interface.Forms;
using MessengerShared;
using MessengerShared.API;
using System.Configuration;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MessengerClient
{
    internal static class Program
    {
        public static class AppContext
        {
            public static AuthService AuthService { get; set; }
            public static NetworkService NetworkService { get; set; }
            public static IProtocol Protocol { get; set; }
            public static ITransport Transport { get; set; }
            public static  event Action? OnDisconnected;

        }

        public static State state = new State();
        [STAThread]
        static void Main()
        {
            AppContext.Transport = new TCPTransport();
            AppContext.Protocol = new JsonProtocol();
            AppContext.AuthService = new AuthService();
            AppContext.NetworkService = new NetworkService();

            ApplicationConfiguration.Initialize();

            Application.ThreadException += (s, e) =>
            {
                Debug.WriteLine($">>> UI ERROR: {e.Exception.Message}");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Debug.WriteLine($">>> FATAL ERROR: {e.ExceptionObject}");
            };

            StartConnection();

            Application.Run(new HelloForm());
            Application.Run(new TestForm());

            AppContext.Transport.Disconnect();
        }

        static void StartConnection()
        {
            if (state.IP == null) return;

            try
            {
                var task = AppContext.Transport.ConnectAsync(state.IP, state.Port);
                task.Wait();

                Task.Run(() => AppContext.Protocol.RunRecieveloop());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> Connection error: {ex.Message}");
            }
        }
    }
}