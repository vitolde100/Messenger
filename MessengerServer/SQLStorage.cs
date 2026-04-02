//Attention, if you accidentally opened this file, then RUN AWAY AND SHOUT AS YOU CAN! THIS IS SQL STORAGE, IT IS VERY SCARY!
//I don`t want to know what is going on here, I just want to get out of here, I want to go back to my safe and cozy JSON storage,
//I don`t want to deal with SQL queries and connections!
//So if you are change smth there please don`t let me know.

using MessengerServer.Data;
using Microsoft.Data.Sqlite;

namespace MessengerServer
{
    internal class SQLStorage : IClientStorage
    {
        private static string Path = "Data\\users\\clients.db";
        private object _lock = new object();
        private Logger m_Logger = Logger.instance;

        SqliteConnection _connection = new SqliteConnection($"Data Source={Path}");

        private static SQLStorage _storage = new SQLStorage();
        public static SQLStorage instance { get { return _storage; } }
        private SQLStorage()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Clients (
               Id TEXT PRIMARY KEY,
               Login TEXT,
               PasswordHash TEXT,
               FriendID TEXT,
               SessionID TEXT   
            );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_login ON Clients(Login);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_session ON Clients(SessionID);";
            cmd.ExecuteNonQuery();
        }

        public bool TryGetClientByID(string ID, out ClientData data)
        {
            lock (_lock)
            {
                data = new ClientData();
                var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Clients WHERE Id = $id";
                cmd.Parameters.AddWithValue("$id", ID);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    data.ID = reader.GetString(0);
                    data.Login = reader.GetString(1);
                    data.Password = reader.GetString(2);
                    data.FriendID = reader.GetString(3);
                    data.SessionID = reader.GetString(4);
                }
                return true;
            }
        }

        public bool TryGetClientBySessionID(string ID, out ClientData data)
        {
            lock (_lock)
            {
                data = new ClientData();
                var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Clients WHERE SessionID = $id";
                cmd.Parameters.AddWithValue("$id", ID);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    data.ID = reader.GetString(0);
                    data.Login = reader.GetString(1);
                    data.Password = reader.GetString(2);
                    data.FriendID = reader.GetString(3);
                    data.SessionID = reader.GetString(4);
                }
                return true;
            }
        }

        public bool TryGetClientByLogin(string Login, out ClientData data)
        {
            lock (_lock)
            {
                data = new ClientData();
                var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Clients WHERE Login = $login";
                cmd.Parameters.AddWithValue("$login", Login);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    data.ID = reader.GetString(0);
                    data.Login = reader.GetString(1);
                    data.Password = reader.GetString(2);
                    data.FriendID = reader.GetString(3);
                    data.SessionID = reader.GetString(4);
                }
                return true;
            }
        }

        public void SaveClient(ClientData user)
        {
            lock (_lock)
            {
                var cmd = _connection.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO Clients (Id, Login, PasswordHash)
                VALUES ($id, $login, $pass, $friID, $SessID);";

                cmd.Parameters.AddWithValue("$id", user.ID);
                cmd.Parameters.AddWithValue("$login", user.Login);
                cmd.Parameters.AddWithValue("$pass", user.Password);
                cmd.Parameters.AddWithValue("$friID", user.FriendID);
                cmd.Parameters.AddWithValue("$SessID", user.SessionID);

                cmd.ExecuteNonQuery();
            }
        }
    }
}