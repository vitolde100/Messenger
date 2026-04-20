using MessengerClient.Client;
using MessengerClient.Client.Protocol;
using MessengerClient.Client.Services;
using MessengerClient.Client.Transport;
using MessengerShared.API;
using System.IO;
using System.Text.Json;

namespace MessengerClient
{
    internal static class Program
    {
        private class Config
        {
            public string UserId { get; set; }
            public Session Session { get; set; }

            public Config() { }

            public Config(string userId, Session session)
            {
                UserId = userId;
                Session = session;
            }
        }

        [STAThread]
        static void Main()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MessengerClient");

            string path = Path.Combine(dir, "config.json");

            Directory.CreateDirectory(dir);

            ITransport transport = new TCPTransport();
            IProtocol protocol = new JsonProtocol(transport);
            AuthService authService = new AuthService(protocol);
            NetworkService networkService = new NetworkService(protocol, authService);

            ApplicationConfiguration.Initialize();

            LoadConfig(path);

            while (true)
            {
                try
                {
                    if (State.Session == null || !State.isLoggedIn)
                    {
                        Application.Run(new Hello(networkService, transport));
                    }
                    else
                    {
                        Application.Run(new ChatForm(protocol, networkService));
                    }

                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Need relogin")
                    {
                        State.Session = null;
                        State.isLoggedIn = false;
                        continue; 
                    }

                    throw;
                }
            }

            SaveConfig(path);
        }

        private static void LoadConfig(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return;

                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    return;

                var config = JsonSerializer.Deserialize<Config>(json);

                if (config != null)
                {
                    State.UserID = config.UserId;
                    State.Session = config.Session;
                    State.isLoggedIn = config.Session != null;
                }
            }
            catch
            {
                // если файл битый — просто игнорим
            }
        }

        private static void SaveConfig(string path)
        {
            try
            {
                var config = new Config(State.UserID, State.Session);
                File.WriteAllText(path, JsonSerializer.Serialize(config));
            }
            catch
            {
                // не критично
            }
        }
    }
}