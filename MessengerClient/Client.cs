using System.Net.Sockets;

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

        public void Connect(string host, int port)
        {
            m_client.Connect(host, port);
            m_stream = m_client.GetStream();
            m_isConnected = m_stream.CanRead && m_stream.CanWrite ? true : false;
        }

        public void SetName(string name)
        { m_name = name; }
        
    }
}