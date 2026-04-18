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

            var json = JsonSerializer.Serialize<Request>(request);
            await _transport.SendAsync(json);
        }

        public async Task<Responce> SendAndReciveAsync(Request request)
        {
            request.Number = Interlocked.Increment(ref packageCounter);
            var tcs = new TaskCompletionSource<Responce>();
            _pendingTasks.TryAdd(request.Number, tcs);

            var json = JsonSerializer.Serialize<Request>(request);
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
                    var envelope = JsonSerializer.Deserialize<Envelope>(msg);

                    switch (envelope.Type)
                    {
                        case "response":
                            var response = envelope.Payload.Deserialize<Responce>();
                            HandleResponse(response);
                            break;

                        case "chat":
                            var chat = envelope.Payload.Deserialize<ChatMessageData>();
                            MessageReceived?.Invoke(chat);
                            break;

                        case "server":
                            var code = envelope.Payload.Deserialize<ServerCodes>();
                            HandleServerCode(code);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (var tcs in _pendingTasks.Values)
                {
                    tcs.SetException(new Exception("Disconnected"));
                }
                _pendingTasks.Clear();
            }
        }

        private void HandleResponse(Responce response)
        {
            if (_pendingTasks.TryRemove(response.Number, out var tcs))
            {
                tcs.SetResult(response);
            }
        }

        private void HandleServerCode(ServerCodes code)
        {
            Console.WriteLine(code.ToString()); // С-с-сервер-сан не д-доволен мной???!1!!!1 (╯°□°）╯
        }
    }
}
