using MessengerShared;
using System.Net.Sockets;
using System.Text;

namespace MessengerClient
{
    public class Client //CHANGE EXCEPTIONS LATER
    {
        private TcpClient m_client = new TcpClient();
        private NetworkStream? m_stream;
        public event Action<ChatMessage> MessageReceived;

        public Client() { }

        private void SendHandshake()
        {
            if (string.IsNullOrEmpty(Program.NickName) || m_stream == null) return;
            byte[] data = Encoding.UTF8.GetBytes(Program.NickName);
            m_stream.Write(data, 0, data.Length);
        }

        public void TryConnect(string host, int port)
        {
            try
            {
                m_client.Connect(host, port);
                m_stream = m_client.GetStream();
                SendHandshake();
                Program.isConnected = m_stream.CanRead && m_stream.CanWrite ? true : false;
                ReadAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ReadAsync()
        {
            byte[] buffer = new byte[1024];
            while (Program.isConnected && m_stream != null)
            {
                int bytesRead = await m_stream.ReadAsync(buffer, 0, buffer.Length);
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
                m_stream.Write(buffer, 0, buffer.Length);
            }
            return Task.CompletedTask;
        }

        public void SetName(string name)
        { Program.NickName = name; } //<--- Needs Server Changes

        public void Disconnect() //<--- Need Changes IDK 
        {
            try
            {
            Program.isConnected = false;
            m_stream.Close();
            m_client.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}