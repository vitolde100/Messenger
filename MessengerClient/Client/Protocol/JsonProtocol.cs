
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MessengerClient.Client.Protocol
{
    internal class JsonProtocol : IProtocol
    {
        private readonly Transport.ITransport _transport;
        private ConcurrentDictionary<int, TaskCompletionSource<Responce>> _pendingTasks;
        private int packageCounter = 0;

        public event Action<ChatMessageData> MessageReceived;

        public JsonProtocol(Transport.ITransport transport)
        {
            _transport = transport;
            _pendingTasks = new ConcurrentDictionary<int, TaskCompletionSource<Responce>>();
        }

        public async Task SendAsync(Request request)
        {
            request.Number = Interlocked.Increment(ref packageCounter);
            var json = JsonSerializer.Serialize(request);
            await _transport.SendAsync(json);
        }

        public async Task<Responce> SendAndReciveAsync(Request request)
        {
            request.Number = Interlocked.Increment(ref packageCounter);

            var tcs = new TaskCompletionSource<Responce>();
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
                    catch
                    {
                        continue;
                    }

                    if (envelope == null)
                        continue;

                    switch (envelope.Type)
                    {
                        case "response":
                            {
                                var response = ((JsonElement)envelope.Payload).Deserialize<Responce>();
                                if (response != null)
                                    HandleResponse(response);
                                break;
                            }

                        case "chat":
                            {
                                var chat = ((JsonElement)envelope.Payload).Deserialize<ChatMessageData>();
                                if (chat != null)
                                    MessageReceived?.Invoke(chat);
                                break;
                            }

                        case "server":
                            {
                                var code = ((JsonElement)envelope.Payload).Deserialize<ServerCodes>();
                                HandleServerCode(code);
                                break;
                            }
                    }
                }
            }
            catch
            {
                foreach (var tcs in _pendingTasks.Values)
                {
                    tcs.TrySetException(new Exception("Disconnected"));
                }
                _pendingTasks.Clear();
            }
        }

        private void HandleResponse(Responce response)
        {
            if (_pendingTasks.TryRemove(response.Number, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }

        private void HandleServerCode(ServerCodes code)
        {
            if (code == ServerCodes.Disconnected)
                _transport.Disconnect();

            Console.WriteLine(code.ToString());
        }
    }
}