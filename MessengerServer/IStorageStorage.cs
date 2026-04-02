using MessengerServer.Data;
using MessengerShared.API;

namespace MessengerServer
{
    internal interface IStorageStorage
    {
        public bool TryGetClientByID(string ID, out ClientData data);

        public bool TryGetClientByLogin(string Login, out ClientData data);

        public void SaveClient(ClientData user);


        public void SaveSession(int userId, string openedKey, string closedKey, DateTime expires);

        public void SaveSession(int userId, Session session);

        public Session GetSessionByOpenedKey(string openedKey);

        public List<Session> GetSessionsById(int userId);

        public void DeleteSession(string openedKey);

    }
}
