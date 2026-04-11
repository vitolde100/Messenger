using BCrypt.Net;
using MessengerServer.Data;
using MessengerShared.API;
using MessengerShared.Requests;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Login : IRequestHandler
    {
        IStorage _storage;
        public Login(IStorage storage) : base()
        {
            _storage = storage;
        }

        public override Responce HandleRequest(JsonElement Data)
        {
            var handshake = JsonSerializer.Deserialize<ClientData>(Data);
            if (!Validate(Data)) return null;
            var user = _storage.GetClientByLogin(handshake.Login);

            if (user == null || 
                BCrypt.Net.BCrypt.Verify(handshake.Password,user.Password))
                return null;

            string accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Session session = new Session(accessToken, refreshToken, user.ID);
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

            return new Responce
            {
                Type = "LoginResponce",
                Data = session.ConvertToPackage()
            };
        }
    }
}
