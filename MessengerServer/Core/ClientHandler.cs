using MessengerServer.Core;
using MessengerServer.Data;
using MessengerServer.RequestHandlers;
using MessengerShared;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using MessengerShared.Requests.Data.Formats;
using MessengerShared.Requests.Enums;
using System.Net.Sockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

internal class ClientHandler
{
    private readonly TcpClient _client;
    private readonly Stream _stream;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    private readonly object _sendLock = new();

    private readonly RequestRouter _requestRouter;
    private readonly Logger _logger = Logger.instance;

    public ClientContext Context { get; private set; }

    private bool _isConnected = true;

    public event Action<ClientHandler>? OnClientDead;

    TimeSpan MSGCooldown = TimeSpan.FromSeconds(0f);
    DateTime LastMSGTime = DateTime.MinValue;

    const int MaxErrorCount = 10;
    int ErrorCount = 0;

    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {

        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public ClientHandler(TcpClient client, Stream stream, RequestRouter router)
    {
        _client = client;
        _stream = stream;
        _requestRouter = router;

        _reader = new StreamReader(_stream, Encoding.UTF8);
        _writer = new StreamWriter(_stream, Encoding.UTF8)
        {
            AutoFlush = true
        };

        Context = new ClientContext();
        _isConnected = true;
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

                string? msg = await _reader.ReadLineAsync();

                if (msg == null)
                {
                    _logger.log("Thread Closed Disconnect", GetType().Name);
                    await Disconnect(ServerCodes.Disconnected);
                    return;
                }

                var now = DateTime.UtcNow;
                if (now - LastMSGTime < MSGCooldown)
                {
                    ErrorCount++;
                    await Send(ServerCodes.TooManyRequests);
                    continue;
                }
                LastMSGTime = now;

                _logger.log(msg, GetType().Name);

                Request? request;
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

                if (response.Error == ServerCodes.BadRequest)
                    ErrorCount++;

                await Send(response);
            }
        }
        catch (Exception ex)
        {
            _logger.log(ex.ToString(), GetType().Name);
            await Disconnect(ServerCodes.Disconnected, ex.Message);
        }
    }

    public Task Send(ServerCodes? code)
    {
        return Send(new Code(code!.Value));
    }

    public async Task Send(IEnvelopePayload obj)
    {
        if (!_isConnected) return;

        var envelope = new Envelope(obj);
        var json = JsonSerializer.Serialize(envelope, _jsonOptions);

        try
        {
            lock (_sendLock)
            {
                _writer.WriteLine(json);
            }

            _logger.log($"Sended: {json}", GetType().Name);
        }
        catch (Exception ex)
        {
            _logger.log($"Send error: {ex.Message}", GetType().Name);
        }
    }

    public void Deauthenticate()
    {
        Context.UserID = null;
        Context.AccessToken = null;
    }

    public async Task Disconnect(ServerCodes? code, string? ex = null)
    {
        code ??= ServerCodes.Disconnected;

        try { await Send(code); } catch { }

        _isConnected = false;

        try { _stream.Dispose(); } catch { }
        try { _client.Dispose(); } catch { }

        if (ex != null)
            _logger.log($"Client {Context.UserID} disconnected with error: {ex}", GetType().Name);
        else
            _logger.log($"Client {Context.UserID} disconnected", GetType().Name);

        OnClientDead?.Invoke(this);
    }

    public void ForceDisconnect()
    {
        try
        {
            _isConnected = false;
            _stream?.Dispose();
            _client?.Close();
        }
        catch { }
    }
}