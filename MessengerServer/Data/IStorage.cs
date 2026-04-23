using MessengerShared.API;

namespace MessengerServer.Data
{
    internal interface IStorage
    {
        ClientData GetClientByID(string ID);

        ClientData GetClientByLogin(string Login);

        void SaveClient(ClientData user);

        void DeleteClient(string UserId);

        Session GetSessionByAccessToken(string accessToken);

        List<Session> GetSessionsById(string userId);

        void SaveSession(Session session);
        
        void SaveSession(string userId, string accessToken, string refreshToken, DateTime accessExpires, DateTime refreshExpires);

        void RemoveSession(string accessToken);
    }
}