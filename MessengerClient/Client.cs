using MessengerShared;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MessengerClient
{
    public class Client //CHANGE EXCEPTIONS LATER
    {
        private TcpClient _client = new TcpClient();
        private Stream _stream;
        public event Action<ChatMessage> MessageReceived;

        static bool ValidateServerCertificate(
            object sender,
            X509Certificate cert,
            X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }

        public Client() { }

        private void SendHandshake()
        {
            if (string.IsNullOrEmpty(Program.NickName) || _stream == null) return;
            byte[] data = Encoding.UTF8.GetBytes(Program.NickName);
            _stream.Write(data, 0, data.Length);
        }

        public async void ConnectAsync(string host, int port)
        {
            try
            {
                await _client.ConnectAsync(host, port);

                var ssl = new SslStream(
                _client.GetStream(),
                false,
                ValidateServerCertificate);

                await ssl.AuthenticateAsClientAsync(host);

                _stream = ssl;
                SendHandshake();
                Program.isConnected = _stream.CanRead && _stream.CanWrite ? true : false;
                await ReadAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ReadAsync()
        {
            byte[] buffer = new byte[1024];
            while (Program.isConnected && _stream != null)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead <= 0)
                {
                    Disconnect();
                    break;
                }

                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (ChatMessage.TryParse(msg, out var message))
                    MessageReceived?.Invoke(message);
            }
        }

        public Task SendMessage(ChatMessage message)
        {
            if (Program.isConnected)
            {
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(message.ToString());
                _stream.Write(buffer, 0, buffer.Length);
            }
            return Task.CompletedTask;
        }

        public void Disconnect() 
        {
            try
            {
                Program.isConnected = false;
                _stream.Dispose();
                _client.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}