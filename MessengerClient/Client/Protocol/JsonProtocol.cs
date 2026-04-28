using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using MessengerShared.Requests.Enums;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MessengerClient.Client.Protocol
{
    internal class JsonProtocol : IProtocol
    {
        private readonly ConcurrentDictionary<EnvelopeTypes, Func<JsonElement, Task>> _handlers;
        private readonly Transport.ITransport _transport = Program.AppContext.Transport;

        private readonly ConcurrentDictionary<int, TaskCompletionSource<Response>> _pendingTasks;
        private int packageCounter = 0;

        private int _disconnected = 0;

        public event Action<ChatMessageData>? OnMessageReceived;
        public event Action? OnDisconnected;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public JsonProtocol()
        {
            _pendingTasks = new ConcurrentDictionary<int, TaskCompletionSource<Response>>();

            _handlers = new()
            {
                [EnvelopeTypes.Response] = HandleResponse,
                [EnvelopeTypes.Message] = HandleMessage,
                [EnvelopeTypes.Code] = HandleServerCode
            };
        }

        public async Task<Response?> SafeSendAsync(Request request)
        {
            try
            {
                return await SendAndReciveAsync(request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> SafeSend error: {ex.Message}");
                return null;
            }
        }

        public async Task<Response> SendAndReciveAsync(Request request)
        {
            if (_disconnected == 1)
                throw new Exception("Not connected");

            request.Number = Interlocked.Increment(ref packageCounter);

            var tcs = new TaskCompletionSource<Response>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            if (!_pendingTasks.TryAdd(request.Number, tcs))
                throw new Exception("Failed to track request");

            try
            {
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                await _transport.SendAsync(json);
            }
            catch (Exception ex)
            {
                _pendingTasks.TryRemove(request.Number, out _);
                tcs.TrySetException(ex);
                throw;
            }

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(5000));

            if (completed != tcs.Task)
            {
                _pendingTasks.TryRemove(request.Number, out _);
                tcs.TrySetException(new TimeoutException());
                throw new TimeoutException("Server did not respond");
            }

            return await tcs.Task;
        }

        public async Task RunRecieveloop()
        {
            try
            {
                while (_disconnected == 0)
                {
                    var msg = await _transport.ReceiveAsync();

                    Debug.WriteLine(">>>>>>> RAW IN: " + msg);
                    if (string.IsNullOrEmpty(msg))
                        throw new Exception("Disconnected");

                    Envelope? envelope;

                    try
                    {
                        envelope = JsonSerializer.Deserialize<Envelope>(msg);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($">>> JSON ERROR: {ex.Message}");
                        continue;
                    }

                    if (envelope == null)
                        continue;

                    if (_handlers.TryGetValue(envelope.Type, out var handler))
                        await handler((JsonElement)envelope.Payload);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> ReceiveLoop stopped: {ex.Message}");
            }

            Disconnect();
        }

        private Task HandleResponse(JsonElement payload)
        {
            try
            {
                var response = JsonSerializer.Deserialize<Response>(payload);

                if (response != null &&
                    _pendingTasks.TryRemove(response.Number, out var tcs))
                {
                    tcs.TrySetResult(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> HandleResponse error: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private Task HandleMessage(JsonElement payload)
        {
            try
            {
                var message = JsonSerializer.Deserialize<ChatMessageData>(payload);
                if (message != null)
                    OnMessageReceived?.Invoke(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> HandleMessage error: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private Task HandleServerCode(JsonElement payload)
        {
            try
            {
                var code = JsonSerializer.Deserialize<ServerCodes>(payload);

                if (code == ServerCodes.Disconnected ||
                    code == ServerCodes.TooManyErrors)
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> HandleCode error: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            foreach (var tcs in _pendingTasks.Values)
            {
                tcs.TrySetException(new Exception("Disconnected"));
            }

            _pendingTasks.Clear();

            try { _transport.Disconnect(); } catch { }

            OnDisconnected?.Invoke();
        }
    }
}