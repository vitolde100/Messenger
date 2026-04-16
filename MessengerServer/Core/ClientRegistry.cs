using MessengerServer.Requests;
using Microsoft.Win32;
using System.Collections.Concurrent;

namespace MessengerServer.Core
{
    internal class ClientGroup
    {
        private int maxId = 1;
        private List<ClientHandler> ClientHandlers = new List<ClientHandler>();

        public void Add(ClientHandler handler)
        {
            handler.Context.RegistryID = maxId;
            maxId++;
        }

        public List<ClientHandler> Get()
        {
            return ClientHandlers;
        }

        public ClientHandler Get(int RegistryID)
        {
            foreach (ClientHandler handler in ClientHandlers)
            {
                if (handler.Context.RegistryID == RegistryID) return handler;
            }
            return null;
        }

        public void Remove(int id)
        {
            foreach (ClientHandler handler in ClientHandlers)
            {
                if (handler.Context.RegistryID == id) ClientHandlers.Remove(handler);
            }
        }
    }

    internal class ClientRegistry
    {
        public static readonly ClientRegistry instance = new ClientRegistry();
        ConcurrentDictionary<string, ClientGroup> m_clients;
        Logger m_logger = Logger.instance;

        ClientRegistry()
        {
            m_clients = new ConcurrentDictionary<string, ClientGroup>();
            m_logger.log("Registry Initialized",this.GetType().Name);
        }

        public void Add (ClientHandler handler)
        {
            if (!m_clients.ContainsKey(handler.Context.UserID))
            {
                var group = new ClientGroup();
                group.Add(handler);
                m_clients.TryAdd(handler.Context.UserID, group);
            }
            else m_clients[handler.Context.UserID].Add(handler);
            m_logger.log("Added " + handler.Context.UserID + "\r", this.GetType().Name);
        }

        public List<ClientHandler> GetClient(string UserID)
        {
            try
            {
                return m_clients[UserID].Get();
            }
            catch
            {
                return null;
            }
        }

        public void Remove(ClientContext context)
        {
            try
            {
                m_clients[context.UserID].Remove(context.RegistryID);
                    
                m_logger.log("Deleted " + context.UserID + "\r", this.GetType().Name);
            }
            catch (Exception e)
            {
                m_logger.log(e.Message, this.GetType().Name);
            }
        }

        public void DisconnectAll()
        {
            throw new NotImplementedException(); //Я silly глюпий не знаю что и как тут делать у меня ключ и 2 раза значение
        }
    }
}
