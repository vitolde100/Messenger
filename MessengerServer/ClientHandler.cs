using MessengerShared;
using System.Net.Sockets;
using System.Text;

namespace MessengerServer
{
    internal class ClientHandler
    {
        bool m_isConnected;
        string m_name;
        TcpClient m_client;
        NetworkStream m_stream;
        Logger m_logger = Logger.instance;
        public event Action<string, ClientHandler> OnClientConnected;
        public event Action<ClientHandler, ChatMessage> OnMessageRecieved;
        public event Action<string> OnClientDead;

        TimeSpan MSGCooldown = TimeSpan.FromSeconds(1.5f);

        public ClientHandler(TcpClient client)
        {
            m_client = client;
            m_stream = m_client.GetStream();
            m_isConnected = m_stream.CanRead && m_stream.CanWrite ? true : false;
        }

        private bool CheckHandshake()
        {
            try
            {
                m_stream.ReadTimeout = 5000;
                byte[] buffer = new byte[MessagingConsts.MaxNameLength];
                int bytesRead = m_stream.Read(buffer, 0, buffer.Length);

                if (bytesRead <= 0)
                {
                    m_logger.log("No Handhake", this.GetType().Name);
                    return false;
                }
                else
                {
                    m_name = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (!string.IsNullOrEmpty(m_name) && m_name.Length <= MessagingConsts.MaxNameLength)
                    {
                        return true;
                    }
                    m_logger.log("Client bad name:" + m_name.Length, this.GetType().Name);
                    return false;
                }
            }
            catch (IOException)
            {
                m_logger.log("No Handshake Exeption or Timeout!", this.GetType().Name);
                return false;
            }
        }

        public async Task Read()
        {
            int ErrorCount = 0;
            DateTime LastMSGTime = DateTime.MinValue;
            m_isConnected = CheckHandshake();

            if (m_isConnected)
            {
                m_logger.log("Client Connected " + m_name, this.GetType().Name);
                m_stream.ReadTimeout = Timeout.Infinite;

                OnClientConnected?.Invoke(m_name, this);
            }
            else 
            {
                Disconnect("No HandShake");
                return; 
            }
            
            try
            {
                while (m_isConnected)
                {
                    byte[] buffer = new byte[MessagingConsts.MaxLength + MessagingConsts.MaxNameLength];
                    int bytesRead = await m_stream.ReadAsync(buffer, 0, buffer.Length);
                    if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
                    {
                        ErrorCount++;
                        m_logger.log(m_name + ":To fast " + ErrorCount, this.GetType().Name);
                        if (ErrorCount > 2)
                        {
                            Disconnect("Flood");
                            return;
                        }
                    }
                    if (bytesRead <= 0)
                    {
                        Disconnect(m_name + "Disconnects the connection");
                        return;
                    }
                    else
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        ChatMessage? message;
                        
                        if (ChatMessage.TryParse(msg, out message))
                        {
                            OnMessageRecieved?.Invoke(this, message);
                            LastMSGTime = DateTime.UtcNow;
                        }
                        else
                        {
                            Disconnect("Bad Message " + msg);
                            return;
                        }
                    }
                }
                Disconnect(null);
                return;
            }
            catch (Exception ex)
            {
                Disconnect(ex.Message);
                return;
            }
        }

        public void Send(ChatMessage message)
        {
            byte[] msg = UnicodeEncoding.UTF8.GetBytes(message.ToString());
            m_stream.Write(msg, 0, msg.Length);
        }

        public void SendSystemMsg(string text)
        {
            ChatMessage message = new ChatMessage();
            DateTime utcNow = DateTime.UtcNow;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            message.SendTime = utcNow - unixEpoch;
            message.Target = this.GetType().Name;
            message.Sender = this.GetType().Name;
            message.Text = text;

            byte[] msg = Encoding.UTF8.GetBytes(message.ToString());
            m_stream.Write(msg, 0, msg.Length);
        }

        public void Disconnect(string? cause)
        {
            m_isConnected = false;
            m_stream.Close();
            m_client.Close();
            if (cause != null) m_logger.log($"Client {m_name} Disconnected: {cause}", this.GetType().Name);
            else m_logger.log($"Client {m_name} Disconnected", this.GetType().Name);

            OnClientDead?.Invoke(m_name);
        }
    }
}