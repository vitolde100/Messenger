using MessengerServer.Data;
using MessengerShared;
using MessengerServer.RequestHandlers;
using System.Net.Sockets;
using System.Text;
using MessengerShared.Requests;
using System.Text.Json;

namespace MessengerServer
{
    internal class ClientHandler
    {
        TcpClient _client;
        Stream _stream;
        RequestRouter _requestRouter;
        Logger _logger = Logger.instance;
        SessionService _sessionService;
        ClientService _clientService;

        bool _isConnected = true;
        bool _isLoggedIn = false;
        public ClientData User = new ClientData();

        public event Action<string, ClientHandler> OnClientConnected;
        public event Action<ClientHandler, ChatMessageData> OnMessageRecieved;
        public event Action<string> OnClientDead;

        TimeSpan MSGCooldown = TimeSpan.FromSeconds(0.5f);

        DateTime LastMSGTime = DateTime.MinValue;
        const int MaxErrorCount = 10;
        int ErrorCount = 0;

        public ClientHandler(TcpClient client, Stream stream, IStorage repository, RequestRouter router, SessionService sessionService, ClientService clientService)
        {
            _client = client;
            _stream = stream;
            _sessionService = sessionService;
            _clientService = clientService;
            _requestRouter = router;
            _isConnected = _stream.CanRead && _stream.CanWrite ? true : false;
        }

        public async Task Run()
        {
            try
            {
                while (_isConnected)
                {
                    if (ErrorCount > MaxErrorCount)
                    {
                        Disconnect(ServerCodes.TooManyErrors);
                        return;
                    }

                    byte[] buffer = new byte[MessagingConsts.MaxLength + MessagingConsts.MaxNameLength];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        Disconnect(null);
                        return;
                    }
                    
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var request = JsonSerializer.Deserialize<Request>(msg);

                    if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
                    {
                        ErrorCount++;
                        SendSystemMsg(ServerCodes.TooManyRequests);
                        _logger.log(User.Login + ":TooManyRequests " + ErrorCount, this.GetType().Name);
                    }

                    if (request != null)
                    {
                        if(!_sessionService.isSessionValid(request.AccessToken))
                        {
                            SendSystemMsg(ServerCodes.Unauthorized);
                            _logger.log("Client " + User.Login + " now unauthorized!", this.GetType().Name);
                            //Login()
                        }
                        
                        var response = _requestRouter.ProcessRequest(request);
                        SendSystemMsg(JsonSerializer.Serialize(response));
                    }

                    LastMSGTime = DateTime.UtcNow;
                }
                Disconnect(null);
                return;
            }
            catch (Exception ex)
            {
                Disconnect(null, ex.Message);
                return;
            }
        }

        /*
        try
        {
            ChatMessageData request = new ChatMessageData();

            if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
            {
                ErrorCount++;
                SendSystemMsg(ServerCodes.TooManyRequests);
                _logger.log(User.Login + ":TooManyRequests " + ErrorCount, this.GetType().Name);
            }

            if (!ChatMessageData.TryParse(msg, out request))
            {
                SendSystemMsg(ServerCodes.BadRequest);
                _logger.log("Client " + User.Login + " sent bad Request!", this.GetType().Name);
                ErrorCount++;
            }

            var session = _storage.GetSessionByAccessToken(request.AccessToken);

            if (session == null && session.userID != User.ID)
            {
                SendSystemMsg(ServerCodes.Unauthorized);
                _logger.log("Client " + User.Login + " now unauthorized!", this.GetType().Name);
                //Login()
                    
            }

            if (!session.isAccessValid())
            {
                SendSystemMsg(ServerCodes.AccessTokenExpired);
                _logger.log("Client " + User.Login + " AccessToken expired!", this.GetType().Name);
                //Wait RefreshToken or Login()
            }

            OnMessageRecieved?.Invoke(this, request);
            LastMSGTime = DateTime.UtcNow;


        }
        catch (Exception ex)
        {
            _logger.log("Error while sending message: " + ex.Message, this.GetType().Name);
        }*/

        public async void Send(ChatMessageData message)
        {
            Send(message.ToString());
        }

        public async void Send(string message)
        {
            byte[] msg = UnicodeEncoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(msg, 0, msg.Length);
        }

        public void SendSystemMsg(ServerCodes? code)
        {
            try
            {
                Send(code.ToString());
            }
            catch (Exception ex)
            {
                _logger.log("Error while sending system message: " + ex.Message, this.GetType().Name);
            }
        }

        public void SendSystemMsg(string message)
        {
            try
            {
                Send(message);
            }
            catch (Exception ex)
            {
                _logger.log("Error while sending system message: " + ex.Message, this.GetType().Name);
            }
        }

        public async void Disconnect(ServerCodes? code, string? Ex = null)
        {
            if (code != null) SendSystemMsg(code);
            else SendSystemMsg(ServerCodes.Disconnected);

            _isConnected = false;
            await _stream.DisposeAsync();
            _client.Dispose();

            if (Ex != null) _logger.log($"Client {User.ID} disconnected with error: {Ex}", this.GetType().Name);
            else _logger.log($"Client {User.ID} Disconnected", this.GetType().Name);

            OnClientDead?.Invoke(User.ID);
        }
    }
}