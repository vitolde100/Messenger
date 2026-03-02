using MessengerShared;
using System.Net;
using System.Net.Sockets;

namespace MessengerServer
{
    internal class Server
    {
        TcpListener m_listener;
        ClientRegistry m_registry = ClientRegistry.instance;
        Logger m_logger = Logger.instance;
        public bool m_running = true;

        public Server(IPAddress ip, int port)
        {
            m_listener = new TcpListener(ip, port);
        }

        public async Task Run()
        {
            m_logger.log("Server Started\n", this.GetType().Name);
            m_listener.Start();
            while (m_running)
            {
                try
                {
                    TcpClient client = await m_listener.AcceptTcpClientAsync();
                    ClientHandler handler = new ClientHandler(client);
                    handler.OnClientConnected += OnClientConnected;
                    handler.OnMessageRecieved += OnMessageReceived;
                    handler.OnClientDead += OnClientDead;
                    handler.Read();
                    
                }
                catch (Exception ex)
                {
                    m_logger.log("А Я УЕБАН!\n", this.GetType().Name);
                    m_logger.log(ex.Message, this.GetType().Name);
                }
            }
            m_logger.log("А Я УЕБАН!\n", this.GetType().Name);
        }

        private void OnClientConnected(string name, ClientHandler client)
        {
            m_registry.Add(name, client);
        }

        private void OnMessageReceived(ClientHandler senderHandler, ChatMessage message)
        {
            ClientHandler client = m_registry.GetClient(message.Target);

            if (client != null)
            {
                client.Send(message);
                senderHandler.SendSystemMsg("0");
            }
            else
            {
                senderHandler.SendSystemMsg("No Target Client"); //<--- Del later (For testing)
                m_logger.log("No Target Client", this.GetType().Name);
            }
        }

        private void OnClientDead(string name)
        {
            m_registry.Remove(name);
            
        }

        public void Stop()
        {
            try
            {
                m_listener.Stop();
            }
            catch (Exception ex)
            {
                m_logger.log(ex.Message, this.GetType().Name);
            }
            m_registry.DisconnectAll();
            m_logger.log("Server Closed\n", this.GetType().Name);
        }
    }
}
