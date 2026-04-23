using MessengerServer.Core;
using MessengerServer.RequestHandlers;
using MessengerServer.Requests;
using MessengerShared;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

internal class ClientHandler
{
    TcpClient _client;
    Stream _stream;
    RequestRouter _requestRouter;
    Logger _logger = Logger.instance;

    public ClientContext Context { get; private set; }

    bool _isConnected = true;

    public event Action<ClientHandler> OnClientDead;

    TimeSpan MSGCooldown = TimeSpan.FromSeconds(0f);

    DateTime LastMSGTime = DateTime.MinValue;
    const int MaxErrorCount = 10;
    int ErrorCount = 0;

    public ClientHandler(TcpClient client, Stream stream, RequestRouter router)
    {
        _client = client;
        _stream = stream;
        _requestRouter = router;
        _isConnected = _stream.CanRead && _stream.CanWrite;
        Context = new ClientContext();
    }

    public async Task Run()
    {
        try
        {
            _logger.log($"Client connected", GetType().Name);
            while (_isConnected)
            {
                if (ErrorCount > MaxErrorCount)
                {
                    await Disconnect(ServerCodes.TooManyErrors);
                    return;
                }

                byte[] buffer = new byte[MessagingConsts.MaxLength + MessagingConsts.MaxNameLength];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead <= 0)
                {
                    await Disconnect(ServerCodes.Disconnected);
                    return;
                }

                var now = DateTime.UtcNow;
                if (now - LastMSGTime < MSGCooldown)
                {
                    ErrorCount++;
                    await Send(ServerCodes.TooManyRequests);
                    _logger.log($"{Context.UserID}: TooManyRequests {ErrorCount}", GetType().Name);
                }
                LastMSGTime = now;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                _logger.log(msg, GetType().Name);

                Request? request = null;
                try
                {
                    request = JsonSerializer.Deserialize<Request>(msg);
                }
                catch
                {
                    await Send(ServerCodes.BadRequest);
                    ErrorCount++;
                    continue;
                }

                if (request == null)
                {
                    await Send(ServerCodes.BadRequest);
                    ErrorCount++;
                    continue;
                }

                var response = _requestRouter.ProcessRequest(request, this);

                if (response.Error == ServerCodes.BadRequest) ErrorCount++;

                await Send(response);
            }

            await Disconnect(ServerCodes.Disconnected);
        }
        catch (Exception ex)
        {
            _logger.log(ex.ToString(), GetType().Name);
            await Disconnect(ServerCodes.Disconnected, ex.Message);
        }
    }

    public async Task Send(object obj)
    {
        if (!_isConnected || !_stream.CanWrite)
            return;

        var envelope = BuildEnvelope(obj);

        try
        {
            byte[] msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(envelope));
            await _stream.WriteAsync(msg, 0, msg.Length);
            _logger.log($"Sended: {JsonSerializer.Serialize(envelope)}", GetType().Name);
        }
        catch (Exception ex)
        {
            _logger.log($"Send error : {ex.Message}", GetType().Name);
        }
    }

    private Envelope BuildEnvelope(object payload)
    {
        var envelope = new Envelope
        {
            Type = payload switch
            {
                Response => "response",
                ChatMessageData => "chat",
                ServerCodes => "server",
                _ => "unknown"
            },
            Payload = payload
        };
        return envelope;
    }

    public void Deauthenticate()
    {
        Context.UserID = null;
        Context.AccessToken = null;
    }

    public async Task Disconnect(ServerCodes? code, string? ex = null)
    {
        code ??= ServerCodes.Disconnected;

        try
        {
            await Send(code);
        }
        catch { }

        _isConnected = false;

        try { await _stream.DisposeAsync(); } catch { }
        try { _client.Dispose(); } catch { }

        if (ex != null)
            _logger.log($"Client {Context.UserID} disconnected with error: {ex}", GetType().Name);
        else
            _logger.log($"Client {Context.UserID} disconnected", GetType().Name);

        OnClientDead?.Invoke(this);
    }
}