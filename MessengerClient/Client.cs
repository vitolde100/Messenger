using MessengerShared;
using System.Net.Sockets;
using System.Text;

namespace MessengerClient
{
    public class Client //CHANGE EXCEPTIONS LATER
    {
        private string m_name = "";
        private TcpClient m_client = new TcpClient();
        private NetworkStream? m_stream;
        public event Action<ChatMessage> MessageReceived;

        public Client() { }

        private void SendHandshake()
        {
            m_stream.Write(UTF8Encoding.UTF8.GetBytes(m_name), 0, m_name.Length);
        }

        public void TryConnect(string host, int port)
        {
            try
            {
                m_client.Connect(host, port);
                m_stream = m_client.GetStream();
                SendHandshake();
                Program.isConnected = m_stream.CanRead && m_stream.CanWrite ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Read()
        {
            while (Program.isConnected)
            {
                byte[] buffer = new byte[1024];
                try
                {
                    int bytesRead = m_stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        Disconnect();
                    }
                    else
                    {
                        string msg = UTF8Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        ChatMessage? message;
                        if (!ChatMessage.TryParse(msg,out message))
                        {
                            //Check Server MSG
                        }
                        MessageReceived?.Invoke(message);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Task SendMessage(ChatMessage message)
        {
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(message.ToString());
                m_stream.Write(buffer, 0, buffer.Length);
                return Task.CompletedTask;
        }

        public void SetName(string name)
        { m_name = name; } //<--- Needs Server Changes

        public void Disconnect() //<--- Needs Changes IDK 
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