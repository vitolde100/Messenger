using MessengerServer.Data;

namespace MessengerServer
{
    internal interface IClientStorage
    {
        public bool TryGetClientByID(string ID, out ClientData data);

        public bool TryGetClientBySessionID(string ID, out ClientData data);

        public bool TryGetClientByLogin(string Login, out ClientData data);

        public void SaveClient(ClientData user);
    }
}
