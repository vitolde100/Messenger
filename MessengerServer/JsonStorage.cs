using MessengerShared;
using System.Text.Json;

namespace MessengerServer
{
    internal class JsonStorage : IClientStorage
    {
        private string Path = "Data/users/";
        private object _lock = new object();
        private Logger m_Logger = Logger.instance;

        private static JsonStorage _storage = new JsonStorage();
        public static JsonStorage instance { get { return _storage; } }

        private JsonStorage()
        {

        }

        public ClientData GetClient(string UID)
        {
            ClientData user = new ClientData();
            try
            {
                string jsonString = File.ReadAllText(Path + UID + ".json");
                user = JsonSerializer.Deserialize<ClientData>(jsonString)!;
                m_Logger.log("Successfully found", this.GetType().Name);
            }
            catch (Exception ex)
            {
                m_Logger.log("Search error: " + ex.Message, this.GetType().Name);
            }
            return user;
        }

        public ClientData GetClient(string login, string password)
        {
            ClientData user = new ClientData();
            try
            {
                string jsonString = File.ReadAllText(Path + login + ".json");
                user = JsonSerializer.Deserialize<ClientData>(jsonString)!;
                if (user.Password != password)
                {
                    m_Logger.log("Wrong password!", this.GetType().Name);
                    return null;
                }
                m_Logger.log("Successfully found", this.GetType().Name);
            }
            catch (Exception ex)
            {
                m_Logger.log("Search error: " + ex.Message, this.GetType().Name);
                return null;
            }
            return user;
        }

        public void SaveClient(ClientData user)
        {
            lock (_lock)
            {
                try
                {
                    string jsonString = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
                    Directory.CreateDirectory("data");
                    File.WriteAllText(Path + user.ID + ".json", jsonString);
                    m_Logger.log("Successfully saved", this.GetType().Name);
                }
                catch (IOException ex) 
                {
                    m_Logger.log("Save error: " + ex.Message, this.GetType().Name);
                }
            }
        }
    }
}
