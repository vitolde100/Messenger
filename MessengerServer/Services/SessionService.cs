using MessengerServer.Core;
using MessengerServer.Data;
using MessengerShared.API;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

namespace MessengerServer.Services
{
    internal class SessionService
    {
        private IStorage _storage;
        private Logger _logger = Logger.instance;

        public SessionService(IStorage storage)
        {
            _storage = storage;
        }

        public Session CreateSession(string userID)
        {
            string accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Session session = new Session(accessToken, refreshToken, userID);

            while (true) 
            {
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
            }
            return session;
        }

        public bool isSessionAccessValid(string? accessToken)
        {
            if (accessToken == null) return false;
            var session = _storage.GetSessionByAccessToken(accessToken);
            return !(session == null || session.IsAccessExpired());
        }

        public bool isSessionRefreshValid(string? accessToken)
        {
            if (accessToken == null) return false;
            var session = _storage.GetSessionByAccessToken(accessToken);
            return !(session == null || session.IsRefreshExpired());
        }

        public Session? GetSessionByAccessToken(string? accessToken)
        {
            if (accessToken == null) return null;
            var session = _storage.GetSessionByAccessToken(accessToken);

            return session;
        }

        public void Remove(string accessToken)
        {
            try
            {
                _storage.RemoveSession(accessToken);
            }
            catch (Exception ex) 
            {
                _logger.log(ex.Message, GetType().Name);
            }
        }
    }
}