using System.Net.Sockets;
using System.Text;

namespace MessengerClient
{
    public class Client
    {
        private bool m_isConnected;
        private string m_name;
        private TcpClient m_client = new TcpClient();
        private NetworkStream m_stream;
        public Client()
        {
        }

        private void SendHandshake()
        {
            m_stream.Write(UTF8Encoding.UTF8.GetBytes(m_name), 0, m_name.Length);
        }

        public void Connect(string host, int port)
        {
            try
            {
                m_client.Connect(host, port);
                m_stream = m_client.GetStream();
                SendHandshake();
                m_isConnected = m_stream.CanRead && m_stream.CanWrite ? true : false;
            }
            catch
            {

            }
        }

        public void SetName(string name)
        { m_name = name; }
        
    }
}