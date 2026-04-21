using MessengerShared.Requests.Data;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MessengerClient.Client.Transport
{
    public class TCPTransport : ITransport
    {
        public bool isConnected { get; private set; }
        TcpClient _client;
        Stream _stream;

        public TCPTransport() { }

        static bool ValidateServerCertificate(
            object sender,
            X509Certificate cert,
            X509Chain chain,
            SslPolicyErrors errors)
        {
            return true; //I know, it`s useless ¯\_(ツ)_/¯
        }

        public async Task ConnectAsync(string host, int port)
        {
            if (isConnected) Disconnect();
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(host, port);

                var ssl = new SslStream(
                _client.GetStream(),
                false,
                ValidateServerCertificate);

                await ssl.AuthenticateAsClientAsync(host);

                _stream = ssl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            isConnected = true;
        }

        public async Task<string> ReceiveAsync()
        {
            try
            {
                if (_client != null || _client.Connected)
                { 
                    byte[] buffer = new byte[4096];
                    string message = _stream.ReadAsync(buffer, 0, buffer.Length).ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            int bytesRead = t.Result;
                            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }).Result;
                    return message;
                }
                isConnected = false;
                return string.Empty;
            }
            catch
            {
                isConnected = false;
                return string.Empty;
            }
        }

        public async Task SendAsync(string message)
        {
            if (_client != null || isConnected)
            {
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(message.ToString());
                await _stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public void Disconnect()
        {
            try
            {
                isConnected = false;
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
