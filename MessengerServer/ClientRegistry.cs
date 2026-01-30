using System.Collections.Concurrent;

namespace MessengerServer
{
    internal class ClientRegistry
    {
        public static readonly ClientRegistry instance = new ClientRegistry();
        ConcurrentDictionary<string, ClientHandler> m_clients;
        Logger m_logger = Logger.instance;

        ClientRegistry()
        {
            m_clients = new ConcurrentDictionary<string, ClientHandler>();
            m_logger.log("Registry Initialized",this.GetType().Name);
        }

        public void Add(string name, ClientHandler handler)
        {
            m_clients.TryAdd(name, handler);
            m_logger.log("Added " + name + "\r", this.GetType().Name);
        }

        public ClientHandler GetClient(string name)
        {
            try
            {
                return m_clients[name];
            }
            catch
            {
                return null;
            }
        }

        public void Remove(string name)
        {
            try
            {
            m_clients.TryRemove(name,out _);
            m_logger.log("Deleted " + name + "\r", this.GetType().Name);
            }
            catch (Exception e)
            {
                m_logger.log(e.Message, this.GetType().Name);
            }
        }

        public void DisconnectAll()
        {
            foreach (var client in m_clients)
            {
                client.Value.Disconnect();
            }
        }
    }
}
