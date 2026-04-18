using MessengerShared.Requests;

namespace MessengerClient.Client.Protocol
{
    internal interface IProtocol
    {
        Task SendAsync(Request request);
        Task<Responce> SendAndReciveAsync(Request request);
        Task RunRecieveloop();
    }
}
