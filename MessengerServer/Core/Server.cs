using System.Net;
using System.Text.Json;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using MessengerServer.RequestHandlers;
using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using MessengerServer.Data;

namespace MessengerServer.Core
{
    internal class Server
    {
        TcpListener _listener;
        ClientRegistry _registry = ClientRegistry.instance;
        Logger _logger = Logger.instance;
        SessionService _sessionService;
        ClientService _clientService;
        MessagingService _messagingService;
        RequestRouter _router;
        public bool _running = true;
        bool _useTls;
        X509Certificate2 _cert;

        string certPath = Path.Combine(AppContext.BaseDirectory, "certs/server.pfx");
        string certPassword = "123456";

        public Server(IPAddress ip, int port, bool useTls)
        {
            IStorage _sql = new SQLStorage();
            _sessionService = new SessionService(_sql);
            _clientService = new ClientService(_sql);
            _messagingService = new MessagingService(_registry, _clientService, _sessionService);
            _router = new RequestRouter(_sessionService); 
            RequestRegistrar.RegiterAll(_router, _sessionService, _clientService, _messagingService);

            _listener = new TcpListener(ip, port);
            _useTls = useTls;
        }

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

                    ClientHandler handler = new ClientHandler(tcp, stream, _router);
                    handler.OnClientConnected += OnClientConnected;
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
            _registry.Add(client);
        }

        private void OnClientDead(ClientHandler handler)
        {
            _registry.Remove(handler.Context);
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
