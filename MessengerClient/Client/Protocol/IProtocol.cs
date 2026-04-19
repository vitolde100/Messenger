using MessengerShared.Requests;
using MessengerShared.Requests.Data;

namespace MessengerClient.Client.Protocol
{
    public interface IProtocol
    {
        public event Action<ChatMessageData> MessageReceived;
        Task SendAsync(Request request);
        Task<Responce> SendAndReciveAsync(Request request);
        Task RunRecieveloop();
    }
}
