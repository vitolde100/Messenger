using MessengerShared.Requests;
using MessengerShared.Requests.Data;

namespace MessengerClient.Client.Protocol
{
    public interface IProtocol
    {
        public event Action<ChatMessageData> MessageReceived;
        Task<Response> SendAndReciveAsync(Request request);
        Task RunRecieveloop();
    }
}
