//Attention, if you accidentally opened this file, then RUN AWAY AND SHOUT AS YOU CAN! THIS IS SQL STORAGE, IT IS VERY SCARY!
//I don`t want to know what is going on here, I just want to get out of here, I want to go back to my safe and cozy JSON storage,
//I don`t want to deal with SQL queries and connections!
//So if you are change smth there please don`t let me know.

using MessengerServer.Core;
using MessengerShared.API;
using Microsoft.Data.Sqlite;

namespace MessengerServer.Data
{
    internal class SQLStorage : IStorage
    {
        private static string ClientsPath = "Data\\Users\\clients.db";
        private static string SessionsPath = "Data\\Sessions\\sessions.db";
        private object _lock = new object();
        private Logger m_Logger = Logger.instance;

        SqliteConnection _clientsConnection = new SqliteConnection($"Data Source={ClientsPath}");
        SqliteConnection _sessionsConnection = new SqliteConnection($"Data Source={SessionsPath}");

        public SQLStorage()
        {
            EnsureUsersTable();
            EnsureSessionsTable();
            m_Logger.log("SQL Storage initialized", this.GetType().Name);
        }
        
        private void EnsureUsersTable()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ClientsPath));
            _clientsConnection.Open();

            using var cmd = _clientsConnection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
               Id TEXT NOT NULL UNIQUE,
               Login TEXT,
               PasswordHash TEXT,
               FriendID TEXT
            );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_login ON Users(Login);";
            cmd.ExecuteNonQuery();
        }

        private void EnsureSessionsTable()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SessionsPath));
            _sessionsConnection.Open();

            using var cmd = _sessionsConnection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Sessions (
                Id TEXT PRIMARY KEY AUTOINCREMENT,
                UserId TEXT NOT NULL,
                AccessToken TEXT NOT NULL UNIQUE,
                RefreshToken TEXT NOT NULL UNIQUE,
                AccessExpires TEXT NOT NULL,
                RefreshExpires TEXT NOT NULL,
                FOREIGN KEY(UserId) REFERENCES Users(Id)
            );";
            cmd.ExecuteNonQuery();
        }

        public ClientData GetClientByID(string ID)
        {
            lock (_lock)
            {
                var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE Id = $id";
                cmd.Parameters.AddWithValue("$id", ID);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ClientData data = new ClientData
                    {
                        ID = reader.GetString(0),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        FriendID = reader.GetString(3)
                    };
                    return data;
                }
                return null;
            }
        }

        public ClientData GetClientByLogin(string Login)
        {
            lock (_lock)
            {
                var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE Login = $login";
                cmd.Parameters.AddWithValue("$login", Login);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ClientData data = new ClientData
                    {
                        ID = reader.GetString(0),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2),
                        FriendID = reader.GetString(3)
                    };
                    return data;
                }
                return null;
            }
        }

        public void SaveClient(ClientData user)
        {
            lock (_lock)
            {
                var cmd = _clientsConnection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Users (Id, Login, PasswordHash, FriendID)
                VALUES ($id, $login, $pass, $friID);";

                cmd.Parameters.AddWithValue("$id", user.ID);
                cmd.Parameters.AddWithValue("$login", user.Login);
                cmd.Parameters.AddWithValue("$pass", user.Password);
                cmd.Parameters.AddWithValue("$friID", user.FriendID);

                cmd.ExecuteNonQuery();
                m_Logger.log("Client saved: " + user.Login, this.GetType().Name);
            }
        }
        
        public void DeleteClient(string userID)
        {
            lock (_lock)
            {
                using var cmd = _sessionsConnection.CreateCommand();
                cmd.CommandText = "DELETE FROM Users WHERE UserId = @Id;";
                cmd.Parameters.AddWithValue("@Id", userID);
                cmd.ExecuteNonQuery();
                m_Logger.log("Client deleted: " + userID, this.GetType().Name);
            }
        }

        public void SaveSession(string userId, string accessToken, string refreshToken, DateTime accessExpires, DateTime refreshExpires)
        {
            lock (_lock)
            {
                using var cmd = _sessionsConnection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Sessions (UserId, AccessToken, RefreshToken, AccessExpires, RefreshExpires)
                VALUES (@userId, @access, @refresh, @aexp, @rexp);";

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@access", accessToken);
                cmd.Parameters.AddWithValue("@refresh", refreshToken);
                cmd.Parameters.AddWithValue("@aexp", accessExpires.ToString("o"));
                cmd.Parameters.AddWithValue("@rexp", refreshExpires.ToString("o"));

                cmd.ExecuteNonQuery();
                m_Logger.log($"Session saved for user: " + userId, this.GetType().Name);
            }
        }

        public void SaveSession(Session session)
        {
            SaveSession(session.userID, session.accessToken, session.refreshToken, session.access_expires, session.refresh_expires);
        }

        public Session GetSessionByAccessToken(string accessToken)
        {
            lock (_lock)
            {
                using var cmd = _sessionsConnection.CreateCommand();
                cmd.CommandText = @"
                SELECT s.AccessToken, s.RefreshToken, s.AccessExpires, s.RefreshExpires, u.Login
                FROM Sessions s
                JOIN Users u ON u.Id = s.UserId
                WHERE s.AccessToken = @access;
                ";
                cmd.Parameters.AddWithValue("@access", accessToken);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Session(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(4)
                    )
                    {
                        access_expires = DateTime.Parse(reader.GetString(2)),
                        refresh_expires = DateTime.Parse(reader.GetString(3))
                    };
                }
                return null;
            }
        }

        public List<Session> GetSessionsById(string userId)
        {
            var list = new List<Session>();
            lock (_lock)
            {
                using var cmd = _sessionsConnection.CreateCommand();
                cmd.CommandText = @"
                SELECT s.AccessToken, s.RefreshToken, s.AccessExpires, s.RefreshExpires, u.Login
                FROM Sessions s
                JOIN Users u ON u.Id = s.UserId
                WHERE s.UserId = @userId;
                ";
                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Session(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(4)
                    )
                    {
                        access_expires = DateTime.Parse(reader.GetString(2)),
                        refresh_expires = DateTime.Parse(reader.GetString(3))
                    });
                }
            }
            return list;
        }

        public void DeleteSession(string userID)
        {
            lock (_lock)
            {
                using var cmd = _sessionsConnection.CreateCommand();
                cmd.CommandText = "DELETE FROM Sessions WHERE UserId = @Id;";
                cmd.Parameters.AddWithValue("@Id", userID);
                cmd.ExecuteNonQuery();
                m_Logger.log("Session deleted for user: " + userID, this.GetType().Name);
            }
        }
    }
}