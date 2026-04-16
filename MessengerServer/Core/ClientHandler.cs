using MessengerShared.Requests.Data;
using MessengerServer.RequestHandlers;
using MessengerServer.Requests;
using MessengerShared;
using MessengerShared.Requests;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessengerServer.Core
{
    internal class ClientHandler
    {
        TcpClient _client;
        Stream _stream;
        RequestRouter _requestRouter;
        Logger _logger = Logger.instance;

        public ClientContext Context { get; private set; }

        bool _isConnected = true;

        public event Action<string, ClientHandler> OnClientConnected;
        public event Action<ClientHandler> OnClientDead;

        TimeSpan MSGCooldown = TimeSpan.FromSeconds(0.5f);

        DateTime LastMSGTime = DateTime.MinValue;
        const int MaxErrorCount = 10;
        int ErrorCount = 0;

        public ClientHandler(TcpClient client, Stream stream, RequestRouter router)
        {
            _client = client;
            _stream = stream;
            _requestRouter = router;
            _isConnected = _stream.CanRead && _stream.CanWrite ? true : false;
            Context = new ClientContext();
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
                    
                    if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
                    {
                        ErrorCount++;
                        Send(ServerCodes.TooManyRequests);
                        _logger.log(Context.UserID + ":TooManyRequests " + ErrorCount, this.GetType().Name);
                    }
                    
                    LastMSGTime = DateTime.UtcNow;

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var request = JsonSerializer.Deserialize<Request>(msg);

                    if (request == null)
                    {
                        Send(ServerCodes.BadRequest);
                        ErrorCount++;
                        continue;
                    }
                        
                    var response = _requestRouter.ProcessRequest(request, Context);
                    Send(response);
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

        public async void Send(object message)
        {
            try
            {
                byte[] msg = UnicodeEncoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                await _stream.WriteAsync(msg, 0, msg.Length);
            }
            catch (Exception ex)
            {
                _logger.log("Error while sending system message: " + ex.Message, this.GetType().Name);
            }
        }

        public async void Disconnect(ServerCodes? code, string? Ex = null)
        {
            if (code == null) code = ServerCodes.Disconnected;
            byte[] msg = UnicodeEncoding.UTF8.GetBytes(code.ToString());
            
            await _stream.WriteAsync(msg, 0, msg.Length);

            _isConnected = false;
            await _stream.DisposeAsync();
            _client.Dispose();

            if (Ex != null) _logger.log($"Client {Context.UserID} disconnected with error: {Ex}", this.GetType().Name);
            else _logger.log($"Client {Context.UserID} Disconnected", this.GetType().Name);

            OnClientDead?.Invoke(this);
        }
    }
}