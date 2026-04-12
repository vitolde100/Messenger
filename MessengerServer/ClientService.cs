using MessengerServer.Data;
using Microsoft.Data.Sqlite;

namespace MessengerServer
{
    internal class ClientService
    {
        private IStorage _storage;

        public ClientService(IStorage storage)
        {
            _storage = storage;
        }

        public ClientData CreateClient(string Login, string Password)
        {
            ClientData User = new ClientData();
            User.ID = Guid.NewGuid().ToString();
            User.Login = Login;
            User.Password = BCrypt.Net.BCrypt.HashPassword(Password);
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
            return User;
        }

        public ClientData GetClientByID(string id)
        {
            return _storage.GetClientByID(id);
        }

        public ClientData GetClientByLogin(string Login)
        {
            return _storage.GetClientByLogin(Login);
        }
    }
}
