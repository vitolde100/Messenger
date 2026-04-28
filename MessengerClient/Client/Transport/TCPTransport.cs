using MessengerShared.Requests.Data;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MessengerClient.Client.Transport
{
    public class TCPTransport : ITransport
    {
        private readonly object _sync = new();
        private bool _connecting = false;
        public bool IsConnected { get; private set; }
        TcpClient? _client;
        Stream? _stream;
        StreamReader? _reader;

        static bool ValidateServerCertificate(
    object sender,
    X509Certificate cert,
    X509Chain chain,
    SslPolicyErrors errors)
        {
            return true; //I know, it`s useless ¯\_(ツ)_/¯
        }

        public async Task SendAsync(string message)
        {
            Stream? streamCopy;
            lock (_sync)
            {
                if (!IsConnected || _stream == null || _client == null || !_client.Connected)
                    throw new InvalidOperationException("Not connected");

                streamCopy = _stream;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
            try
            {
                await streamCopy.WriteAsync(buffer, 0, buffer.Length);
                await streamCopy.FlushAsync();
            }
            catch (ObjectDisposedException)
            {
                lock (_sync) { IsConnected = false; _stream = null; _client = null; _reader = null; }
                throw;
            }
        }

        public async Task<string> ReceiveAsync()
        {
            StreamReader? reader;

            lock (_sync)
            {
                if (!IsConnected || _reader == null)
                    return string.Empty;

                reader = _reader;
            }

            try
            {
                return await reader.ReadLineAsync() ?? string.Empty;
            }
            catch
            {
                lock (_sync)
                {
                    IsConnected = false;
                }

                return string.Empty;
            }
        }

        public void Disconnect()
        {
            lock (_sync)
            {
                if (!IsConnected) return;

                IsConnected = false;

                try { _stream?.Dispose(); } catch { }
                try { _client?.Close(); } catch { }

                _stream = null;
                _client = null;
                _reader = null;
            }

            Debug.WriteLine(">>> DISCONNECTED");
        }

        public async Task ConnectAsync(string host, int port)
        {
            lock (_sync)
            {
                if (IsConnected || _connecting)
                    return;

                _connecting = true;
            }

            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(host, port);

                var ssl = new SslStream(client.GetStream(), false, ValidateServerCertificate);
                await ssl.AuthenticateAsClientAsync(host);

                lock (_sync)
                {
                    _client = client;
                    _stream = ssl;
                    _reader = new StreamReader(_stream, Encoding.UTF8);
                    IsConnected = true;
                }

                Debug.WriteLine(">>> CONNECT OK");
            }
            finally
            {
                lock (_sync)
                    _connecting = false;
            }
        }
    }
}
