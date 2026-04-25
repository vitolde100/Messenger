
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using MessengerShared.Requests.Enums;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace MessengerClient.Client.Protocol
{
    internal class JsonProtocol : IProtocol
    {
        private readonly ConcurrentDictionary<EnvelopeTypes, Func<JsonElement, Task>> _handlers;
        private readonly Transport.ITransport _transport = Program.AppContext.Transport;
        private ConcurrentDictionary<int, TaskCompletionSource<Response>> _pendingTasks;
        private int packageCounter = 0;

        public event Action<ChatMessageData>? OnMessageReceived;

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

        public async Task<Response> SendAndReciveAsync(Request request)
        {
            request.Number = Interlocked.Increment(ref packageCounter);

            var tcs = new TaskCompletionSource<Response>(TaskCreationOptions.RunContinuationsAsynchronously); 
            _pendingTasks.TryAdd(request.Number, tcs);

            var json = JsonSerializer.Serialize(request);
            await _transport.SendAsync(json);

            return await tcs.Task; 
        }

        public async Task RunRecieveloop()
        {
            try
            {
                while (true)
                {
                    var msg = await _transport.ReceiveAsync();

                    if (string.IsNullOrEmpty(msg))
                        break;

                    Envelope? envelope = null;

                    try
                    {
                        envelope = JsonSerializer.Deserialize<Envelope>(msg);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($">>>>>>>>>>>>>>>>>{ex.Message}");
                        continue;
                    }

                    if (envelope == null) continue;

                    if (_handlers.TryGetValue(envelope.Type, out var handler))
                        await handler((JsonElement)envelope.Payload);
                }
            }
            catch (Exception ex) 
            {
                Debug.Print($">>> {ex.Message}");
            }
            Disconnect();
        }

        private async Task HandleResponse(JsonElement payload)
        {
            var response = JsonSerializer.Deserialize<Response>(payload);
            if (response != null)
            {
                if (_pendingTasks.TryRemove(response.Number, out var tcs))
                {
                    tcs.TrySetResult(response);
                }
            }
        }

        private async Task HandleMessage(JsonElement payload)
        {
            var message = JsonSerializer.Deserialize<ChatMessageData>(payload);
            OnMessageReceived?.Invoke(message);
        }

        private async Task HandleGroupEvent(JsonElement payload)
        {
            //var Data = JsonSerializer.Deserialize<>(payload);
            //
        }

        private async Task HandleServerCode(JsonElement payload)
        {
            var code = JsonSerializer.Deserialize<ServerCodes>(payload);
            switch (code)
            {
                case ServerCodes.Disconnected:
                    Disconnect();
                    break;
                    
                case ServerCodes.TooManyErrors:
                    Disconnect();
                    break;
            }
        }

        private void Disconnect()
        {
            foreach (var tcs in _pendingTasks.Values)
            {
                tcs.TrySetException(new Exception("Disconnected"));
            }
            _pendingTasks.Clear();
            _transport.Disconnect();
        }

    }
}