using MessengerServer.Data;
using MessengerShared.API;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

namespace MessengerServer.Services
{
    internal class SessionService
    {
        private IStorage _storage;

        public SessionService(IStorage storage)
        {
            _storage = storage;
        }

        public Session CreateSession(string userID)
        {
            string accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Session session = new Session(accessToken, refreshToken, userID);

            while (true) //Save Session
                try
                {
                    _storage.SaveSession(session);
                    break;
                }
                catch (SqliteException)
                {
                    session.accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                    session.refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                }
            return session;
        }

        public bool isSessionValid(string accessToken)
        {
            var session = _storage.GetSessionByAccessToken(accessToken);
            if (session == null || session.IsExpired())
                return false;
            return true;
        }

        public void RemoveSession(string accessToken)
        {
            _storage.DeleteSession(accessToken);
        }
    }
}