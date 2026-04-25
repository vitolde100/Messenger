using MessengerServer.Core;
using MessengerShared.API;
using Microsoft.Data.Sqlite;

namespace MessengerServer.Data
{
    internal class SQLStorage : IStorage
    {
        private static string DbPath = "Data/app.db";
        private object _lock = new object();
        private Logger m_Logger = Logger.instance;

        SqliteConnection _connection = new SqliteConnection($"Data Source={DbPath}");

        public SQLStorage()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath));

            _connection.Open();

            EnsureUsersTable();
            EnsureSessionsTable();

            m_Logger.log("SQL Storage initialized", this.GetType().Name);
        }

        private void EnsureUsersTable()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
               Id TEXT PRIMARY KEY,
               Login TEXT NOT NULL UNIQUE,
               PasswordHash TEXT NOT NULL,
               FriendID TEXT NULL
            );";
            cmd.ExecuteNonQuery();
        }

        private void EnsureSessionsTable()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
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
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE Id = $id";
                cmd.Parameters.AddWithValue("$id", ID);

                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return new ClientData
                {
                    ID = reader.GetString(0),
                    Login = reader.GetString(1),
                    Password = reader.GetString(2),
                    FriendID = reader.IsDBNull(3) ? null : reader.GetString(3)
                };
            }
        }

        public ClientData GetClientByLogin(string login)
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE Login = $login";
                cmd.Parameters.AddWithValue("$login", login);

                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return new ClientData
                {
                    ID = reader.GetString(0),
                    Login = reader.GetString(1),
                    Password = reader.GetString(2),
                    FriendID = reader.IsDBNull(3) ? null : reader.GetString(3)
                };
            }
        }

        public void SaveClient(ClientData user)
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Users (Id, Login, PasswordHash, FriendID)
                VALUES ($id, $login, $pass, $friID);";

                cmd.Parameters.AddWithValue("$id", user.ID);
                cmd.Parameters.AddWithValue("$login", user.Login);
                cmd.Parameters.AddWithValue("$pass", user.Password);
                cmd.Parameters.AddWithValue("$friID", (object?)user.FriendID ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                m_Logger.log("Client saved: " + user.Login, this.GetType().Name);
            }
        }

        public void DeleteClient(string userID)
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Users WHERE Id = @Id;";
                cmd.Parameters.AddWithValue("@Id", userID);
                cmd.ExecuteNonQuery();

                m_Logger.log("Client deleted: " + userID, this.GetType().Name);
            }
        }

        public void SaveSession(string userId, string accessToken, string refreshToken, DateTime accessExpires, DateTime refreshExpires)
        {
            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Sessions (UserId, AccessToken, RefreshToken, AccessExpires, RefreshExpires)
                VALUES (@userId, @access, @refresh, @aexp, @rexp);";

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@access", accessToken);
                cmd.Parameters.AddWithValue("@refresh", refreshToken);
                cmd.Parameters.AddWithValue("@aexp", accessExpires.ToString("o"));
                cmd.Parameters.AddWithValue("@rexp", refreshExpires.ToString("o"));

                cmd.ExecuteNonQuery();
                m_Logger.log("Session saved for user: " + userId, this.GetType().Name);
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
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = @"
                SELECT s.AccessToken, s.RefreshToken, s.AccessExpires, s.RefreshExpires, u.Login
                FROM Sessions s
                JOIN Users u ON u.Id = s.UserId
                WHERE s.AccessToken = @access;
                ";

                cmd.Parameters.AddWithValue("@access", accessToken);

                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

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
        }

        public List<Session> GetSessionsById(string userId)
        {
            var list = new List<Session>();

            lock (_lock)
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = @"
                SELECT AccessToken, RefreshToken, AccessExpires, RefreshExpires
                FROM Sessions
                WHERE UserId = @userId;
                ";

                cmd.Parameters.AddWithValue("@userId", userId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Session(
                        reader.GetString(0),
                        reader.GetString(1),
                        userId
                    )
                    {
                        access_expires = DateTime.Parse(reader.GetString(2)),
                        refresh_expires = DateTime.Parse(reader.GetString(3))
                    });
                }
            }

            return list;
        }


        public void RemoveSession(string accessToken)
        {
            lock (_lock)
            {
                try
                {
                    using var cmd = _connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM Sessions WHERE AccessToken = @token;";
                    cmd.Parameters.AddWithValue("@token", accessToken);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        m_Logger.log("Session deleted: " + accessToken, this.GetType().Name);
                    else
                        m_Logger.log("Session not found: " + accessToken, this.GetType().Name);
                }
                catch (Exception ex)
                {
                    m_Logger.log("SQL ERROR (RemoveSession): " + ex.Message, this.GetType().Name);
                }
            }
        }
    }
}