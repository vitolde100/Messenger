using MessengerClient.Client;
using MessengerClient.Client.Protocol;
using MessengerClient.Client.Services;
using MessengerClient.Client.Transport;
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

        public class Config 
        {
            public class inFileSession
            {
                public byte[] accessToken { get; set; }
                public byte[] refreshToken { get; set; }
                public DateTime access_expires { get; set; }
                public DateTime refresh_expires { get; set; }
            }

            public string IP { get; set; } = "192.168.1.2";
            public int Port { get; set; } = 5000;
            public string UserID { get; set; }
            public inFileSession Session { get; set; }

            public Config() { }

            public Config(State state)
            {
                IP = state.IP;
                Port = state.Port;
                if (state.Session != null)
                {
                    UserID = state.Session.userID;
                    Session = new inFileSession()
                    {
                        accessToken = Protect(state.Session.accessToken),
                        access_expires = state.Session.access_expires,
                        refreshToken = Protect(state.Session.refreshToken),
                        refresh_expires = state.Session.refresh_expires
                    };
                }
            }

            public void ToState(out State state)
            {
                state = new State();
                state.IP = IP;
                state.Port = Port;
                state.UserID = UserID;
                state.Session = new Session()
                {
                    accessToken = Unprotect(Session.accessToken),
                    access_expires = Session.access_expires,
                    refreshToken = Unprotect(Session.refreshToken),
                    refresh_expires = Session.refresh_expires,
                    userID = UserID
                };
            }
        }

        public static State state = new State();
        [STAThread]
        static async Task Main()
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

            if (Program.state.IP != null)
            {
                Thread.Sleep(500);
                try
                {
                    await transport.ConnectAsync(Program.state.IP, Program.state.Port);
                    new Thread(() => protocol.RunRecieveloop()).Start();
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.Print($">>>>>>>>>>>>Connection error: {ex}");
#endif
                }
            }

                while (true)
                {
                    try
                    {
                        if (!state.isLoggedIn)
                        {
                            Application.Run(new Hello(networkService, transport, protocol));
                            continue;
                        }
                        else
                        {
                            Application.Run(new ChatForm(protocol, networkService));
                            break;
                        }

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Need relogin")
                        {
                            state.Clear();
                            continue;
                        }

                        throw;
                    }
                }
                SaveConfig(path);
                transport.Disconnect();
            
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
                if (state == null)
                {
                    Debug.Print(">>>>>>>>>Config is null");
                    return;
                }

                config.ToState(out state);

                Debug.Print($">>>>>>>>>>>>UserId: {state.UserID}, Session: {state.Session != null}");

            }
            catch (Exception ex)
            {
                Debug.Print($">>>>>>>>>>>>LoadConfig error: {ex}");
            }
        }

        private static void SaveConfig(string path)
        {
            try
            {
                File.WriteAllText(path, JsonSerializer.Serialize(new Config(state)));
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        public static byte[] Protect(string data)
        {
            return ProtectedData.Protect(
                Encoding.UTF8.GetBytes(data),
                null,
                DataProtectionScope.CurrentUser);
        }

        public static string Unprotect(byte[] data)
        {
            return Encoding.UTF8.GetString(
                ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser));
        }
    }
}