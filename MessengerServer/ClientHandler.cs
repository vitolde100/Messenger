using System.Net.Sockets;
using System.Text;

namespace MessengerServer
{
    struct MessageArguments
    {
        public string Data;
        public string Sender;
    }

    internal class ClientHandler
    {
        bool m_isConnected;
        string m_name;
        TcpClient m_client;
        NetworkStream m_stream;
        ClientRegistry m_registry;
        Logger m_logger = Logger.instance;

        const int NameMaxLength = 32;
        const int MSGMaxLength = 256;
        TimeSpan MSGCooldown = TimeSpan.FromSeconds(1.5f);

        public ClientHandler(TcpClient client)
        {
            m_client = client;
            m_stream = m_client.GetStream();
            m_isConnected = m_stream.CanRead && m_stream.CanWrite ? true : false;
            m_registry = ClientRegistry.instance;
        }

        private bool CheckHandshake()
        {
            try
            {
                m_stream.ReadTimeout = 5000;
                byte[] buffer = new byte[NameMaxLength];
                int bytesRead = m_stream.Read(buffer, 0, buffer.Length);

                if (bytesRead <= 0)
                {
                    m_logger.log("No Handhake", this.GetType().Name);
                    return false;
                }
                else
                {
                    m_name = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (!string.IsNullOrEmpty(m_name) && m_name.Length <= NameMaxLength)
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

        public void Read()
        {
            int ErrorCount = 0;
            DateTime LastMSGTime = DateTime.MinValue;
            m_isConnected = CheckHandshake();
            if (m_isConnected)
            {
                m_logger.log("Client Connected " + m_name, this.GetType().Name);
                m_registry.Add(m_name, this);
                m_stream.ReadTimeout = Timeout.Infinite;
            }
            else
                Disconnect();
            
            while (m_isConnected)
            {
                try
                {
                    byte[] buffer = new byte[MSGMaxLength + NameMaxLength];
                    int bytesRead = m_stream.Read(buffer, 0, buffer.Length);
                    if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
                    {
                        ErrorCount++;
                        m_logger.log(m_name + ":To fast " + ErrorCount, this.GetType().Name);
                        if (ErrorCount > 2)
                        {
                            m_logger.log(m_name + ":Flood", this.GetType().Name);
                            Disconnect();
                            return;
                        }

                    }
                    if (bytesRead <= 0)
                    {
                        m_logger.log(m_name + " Disconnects the connection", this.GetType().Name);
                        Disconnect();
                    }
                    else
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string[] Data = msg.Split('|',2);

                        if (msg.Length <= MSGMaxLength && !string.IsNullOrEmpty(Data[1]) && Data[0].Length <= NameMaxLength)
                        {
                            MessageArguments message = new MessageArguments();
                            message.Sender = m_name;
                            message.Data += Data[1];
                            m_logger.log("[" + m_name + "]->[" + Data[0] + "]:" + message.Data, this.GetType().Name);
                            Send(Data[0], message);
                            LastMSGTime = DateTime.UtcNow;
                        }
                        else
                        {
                            m_logger.log("Client Bad Message", this.GetType().Name);
                            Disconnect();
                        }

                    }
                }
                catch (Exception ex)
                {
                    m_logger.log("Read error: " + ex.Message, this.GetType().Name);
                    Disconnect();
                }
            }
        }

        public void Send(string target, MessageArguments message)
        {
            ClientHandler client = m_registry.GetClient(target);
            if (client != null)
            {
                client.Write(message);
            }
            else
            {
                SendSystemMsg("No Target Client"); //<--- Del later
                m_logger.log("No Target Client", this.GetType().Name);
            }
        }
        
        private void Write(MessageArguments message)
        {
            byte[] msg = Encoding.UTF8.GetBytes("[" + message.Sender + "]" + message.Data);
            m_stream.Write(msg, 0, msg.Length);
        }

        public void SendSystemMsg(string message)
        {
            byte[] msg = Encoding.UTF8.GetBytes("[Server]" + message);
            m_stream.Write(msg, 0, msg.Length);
        }

        public void Disconnect()
        {
            m_isConnected = false;
            m_stream.Close();
            m_client.Close();
            m_logger.log("Client Disconnected", this.GetType().Name);
            if(!string.IsNullOrEmpty(m_name))
                m_registry.Remove(m_name);
        }
    }
}