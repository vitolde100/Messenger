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

        public void Add(string userID, ClientHandler handler)
        {
            m_clients.TryAdd(userID, handler);
            m_logger.log("Added " + userID + "\r", this.GetType().Name);
        }

        public ClientHandler GetClient(string userID)
        {
            try
            {
                return m_clients[userID];
            }
            catch
            {
                return null;
            }
        }

        public void Remove(string userID)
        {
            try
            {
                m_clients.TryRemove(userID,out _);
                m_logger.log("Deleted " + userID + "\r", this.GetType().Name);
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
                client.Value.Disconnect("Ok Bye!!!1!");
            }
        }
    }
}
