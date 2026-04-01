using MessengerShared;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace MessengerServer
{
    internal class Server
    {
        TcpListener _listener;
        ClientRegistry _registry = ClientRegistry.instance;
        Logger _logger = Logger.instance;

        public bool _running = true;
        bool _useTls;
        X509Certificate2 _cert;
        public Server(IPAddress ip, int port, bool useTls)
        {
            _listener = new TcpListener(ip, port);
            _useTls = useTls; 
        }

        string certPath = Path.Combine(AppContext.BaseDirectory, "certs/server.pfx");
        string certPassword = "123456";

        public async Task Run()
        {
            try
            {
                _cert = new X509Certificate2(certPath, certPassword);
                _logger.log("Cert loaded: " + _cert.Subject, this.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.log("Failed to load cert: " + ex.Message, this.GetType().Name);
            }
            _logger.log("Server Started\n", this.GetType().Name);

            _listener.Start();

            while (_running)
            {
                try
                {
                    var tcp = await _listener.AcceptTcpClientAsync();
                    Stream stream;

                    if (_useTls)
                    {
                        var ssl = new SslStream(tcp.GetStream(), false);
                        await ssl.AuthenticateAsServerAsync(_cert);
                        stream = ssl;
                    }
                    else
                    {
                        stream = tcp.GetStream();
                    }

                    ClientHandler handler = new ClientHandler(tcp, stream);
                    handler.OnClientConnected += OnClientConnected;
                    handler.OnMessageRecieved += OnMessageReceived;
                    handler.OnClientDead += OnClientDead;
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await handler.Run();
                        }
                        catch (Exception ex)
                        {
                            _logger.log(ex.Message, this.GetType().Name);
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.log(ex.Message, this.GetType().Name);
                }
            }
        }

        private void OnClientConnected(string id, ClientHandler client)
        {
            _registry.Add(id, client);
        }

        private void OnMessageReceived(ClientHandler senderHandler, ChatMessage message)
        {
            ClientHandler client = _registry.GetClient(message.Target);

            if (client != null)
            {
                client.Send(message);
                senderHandler.SendSystemMsg(ServerCodes.None);
            }
            else
            {
                senderHandler.SendSystemMsg(ServerCodes.NoTargetClient);
                _logger.log("No Target Client", this.GetType().Name);
            }
        } 

        private void OnClientDead(string id)
        {
            _registry.Remove(id);
        }

        public void Stop()
        {
            _running = false;
            try
            {
                _listener.Stop();
            }
            catch (Exception ex)
            {
                _logger.log(ex.Message, this.GetType().Name);
            }
            _registry.DisconnectAll();
            _logger.log("Server Closed\n", this.GetType().Name);
        }
    }
}
