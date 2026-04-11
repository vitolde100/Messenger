using MessengerServer.Data;
using MessengerShared.API;
using MessengerShared.RequestData;
using MessengerShared.Requests;
using Microsoft.Data.Sqlite;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Registration : IRequestHandler
    {
        IStorage _storage;
        ClientData User = new ClientData();
        public Registration(IStorage storage) : base()
        { 
            storage = _storage;
        }

        public override Responce HandleRequest(JsonElement Data)
        {
            var handshake = JsonSerializer.Deserialize<ClientData>(Data);
            if (!Validate(Data)) return null;
            User.ID = Guid.NewGuid().ToString();
            User.Login = handshake.Login;
            User.Password = BCrypt.Net.BCrypt.HashPassword(handshake.Password);
            User.FriendID = null;

            while (true) //Save Client
                try
                {
                    _storage.SaveClient(User);
                    break;
                }
                catch (SqliteException)
                {
                    User.ID = Guid.NewGuid().ToString();
                }

            string accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Session session = new Session(accessToken, refreshToken, User.ID);

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
                Type = "HandshakeResponce",
                Success = true,
                Data = session.ConvertToPackage()
            };
        }
    }
}
