//Attention, if you accidentally opened this file, then RUN AWAY AND SHOUT AS YOU CAN! THIS IS SQL STORAGE, IT IS VERY SCARY!
//I don`t want to know what is going on here, I just want to get out of here, I want to go back to my safe and cozy JSON storage,
//I don`t want to deal with SQL queries and connections!
//So if you are change smth there please don`t let me know.

using MessengerServer.Data;
using MessengerShared.API;
using Microsoft.Data.Sqlite;

namespace MessengerServer
{
    internal class SQLStorage : IStorageStorage
    {
        private static string ClientsPath = "Data\\Users\\clients.db";
        private static string SessionsPath = "Data\\Sessions\\sessions.db";
        private object _lock = new object();
        private Logger m_Logger = Logger.instance;

        SqliteConnection _clientsConnection = new SqliteConnection($"Data Source={ClientsPath}");
        SqliteConnection _sessionsConnection = new SqliteConnection($"Data Source={SessionsPath}");

        private static SQLStorage _storage = new SQLStorage();
        public static SQLStorage instance { get { return _storage; } }
        private SQLStorage()
        {
            EnsureClientsTable();
            EnsureSessionsTable();
        }

        private void EnsureClientsTable()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ClientsPath));
            _clientsConnection.Open();

            using var cmd = _clientsConnection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Clients (
               Id TEXT PRIMARY KEY,
               Login TEXT,
               PasswordHash TEXT,
               FriendID TEXT,
            );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_login ON Clients(Login);";
            cmd.ExecuteNonQuery();
        }

        private void EnsureSessionsTable()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ClientsPath));
            _sessionsConnection.Open();

            using var cmd = _sessionsConnection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                OpenedKey TEXT NOT NULL UNIQUE,
                ClosedKey TEXT NOT NULL,
                Expires TEXT NOT NULL,
                FOREIGN KEY(UserId) REFERENCES Users(Id)
            );";
            cmd.ExecuteNonQuery();
        }


        public bool TryGetClientByID(string ID, out ClientData data)
        {
            lock (_lock)
            {
                data = new ClientData();
                var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Clients WHERE Id = $id";
                cmd.Parameters.AddWithValue("$id", ID);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    data.ID = reader.GetString(0);
                    data.Login = reader.GetString(1);
                    data.Password = reader.GetString(2);
                    data.FriendID = reader.GetString(3);
                }
                return true;
            }
        }

        public bool TryGetClientByLogin(string Login, out ClientData data)
        {
            lock (_lock)
            {
                data = new ClientData();
                var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Clients WHERE Login = $login";
                cmd.Parameters.AddWithValue("$login", Login);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    data.ID = reader.GetString(0);
                    data.Login = reader.GetString(1);
                    data.Password = reader.GetString(2);
                    data.FriendID = reader.GetString(3);
                }
                return true;
            }
        }

        public void SaveClient(ClientData user)
        {
            lock (_lock)
            {
                var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Clients (Id, Login, PasswordHash)
                VALUES ($id, $login, $pass, $friID, $SessID);";

                cmd.Parameters.AddWithValue("$id", user.ID);
                cmd.Parameters.AddWithValue("$login", user.Login);
                cmd.Parameters.AddWithValue("$pass", user.Password);
                cmd.Parameters.AddWithValue("$friID", user.FriendID);

                cmd.ExecuteNonQuery();
            }
        }


        public void SaveSession(int userId, string openedKey, string closedKey, DateTime expires)
        {
            lock (_lock)
            {
                using var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Sessions (UserId, OpenedKey, ClosedKey, Expires)
                VALUES (@userId, @openedKey, @closedKey, @expires);
            ";
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@openedKey", openedKey);
                cmd.Parameters.AddWithValue("@closedKey", closedKey);
                cmd.Parameters.AddWithValue("@expires", expires.ToString("o")); 
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveSession(int userId, Session session)
        {
            lock (_lock)
            {
                using var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Sessions (UserId, OpenedKey, ClosedKey, Expires)
                VALUES (@userId, @openedKey, @closedKey, @expires);
            ";
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@openedKey", session.openedKey);
                cmd.Parameters.AddWithValue("@closedKey", session.closedKey);
                cmd.Parameters.AddWithValue("@expires", session.expires.ToString("o"));
                cmd.ExecuteNonQuery();
            }
        }

        public Session GetSessionByOpenedKey(string openedKey)
        {
            lock (_lock)
            {
                using var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = @"
                SELECT s.OpenedKey, s.ClosedKey, s.Expires, u.Login
                FROM Sessions s
                JOIN Users u ON u.Id = s.UserId
                WHERE s.OpenedKey = @openedKey;
            ";
                cmd.Parameters.AddWithValue("@openedKey", openedKey);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var oKey = reader.GetString(0);
                    var cKey = reader.GetString(1);
                    var expires = DateTime.Parse(reader.GetString(2));
                    var login = reader.GetString(3);

                    var session = new Session(oKey, cKey, login);
                    typeof(Session).GetField("expires", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                    ?.SetValue(session, expires);
                    return session;
                }
                return null;
            }
        }

        public List<Session> GetSessionsById(int userId)
        {
            var list = new List<Session>();
            lock (_lock)
            {
                using var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = @"
                SELECT s.OpenedKey, s.ClosedKey, s.Expires, u.Login
                FROM Sessions s
                JOIN Users u ON u.Id = s.UserId
                WHERE s.UserId = @userId;
            ";
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var oKey = reader.GetString(0);
                    var cKey = reader.GetString(1);
                    var expires = DateTime.Parse(reader.GetString(2));
                    var login = reader.GetString(3);

                    var session = new Session(oKey, cKey, login);
                    typeof(Session).GetField("expires", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                    ?.SetValue(session, expires);

                    list.Add(session);
                }
            }
            return list;
        }

        public void DeleteSession(string openedKey)
        {
            lock (_lock)
            {
                using var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = "DELETE FROM Sessions WHERE OpenedKey = @openedKey;";
                cmd.Parameters.AddWithValue("@openedKey", openedKey);
                cmd.ExecuteNonQuery();
            }
        }
    }
}