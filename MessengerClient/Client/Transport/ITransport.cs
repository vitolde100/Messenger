namespace MessengerClient.Client.Transport
{
    public interface ITransport
    {
        bool IsConnected { get; }
        Task ConnectAsync(string host, int port);
        Task SendAsync(string data);
        Task<string> ReceiveAsync();
        void Disconnect();
    }
}
