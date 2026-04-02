using MessengerServer.Data;
using MessengerShared;
using MessengerShared.API;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MessengerServer
{
    internal class ClientHandler
    {
        TcpClient _client;
        Stream _stream;
        Logger _logger = Logger.instance;
        IStorageStorage _storage;

        bool _isConnected;
        public ClientData User = new ClientData();

        public event Action<string, ClientHandler> OnClientConnected;
        public event Action<ClientHandler, ChatMessage> OnMessageRecieved;
        public event Action<string> OnClientDead;

        TimeSpan MSGCooldown = TimeSpan.FromSeconds(0f);

        public ClientHandler(TcpClient client, Stream stream, IStorageStorage repository)
        {
            _client = client;
            _stream = stream;
            _storage = repository;
            _isConnected = _stream.CanRead && _stream.CanWrite ? true : false;
        }

        private bool Handshake()
        {
            try
            {
                _stream.ReadTimeout = 5000;
                byte[] buffer = new byte[MessagingConsts.MaxNameLength];
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);

                if (bytesRead <= 0)
                {
                    _logger.log("Timeout!", this.GetType().Name);
                    return false;
                }
                else
                {
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    HandshakeMessage handshake; 
                    if(HandshakeMessage.TryParse(msg, out handshake))
                    {
                        string openedKey = Guid.NewGuid().ToString();
                        string closedKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                        var session = new Session(openedKey, closedKey, handshake.Login);
                        string package = session.ConvertToPackage();
                        SendSystemMsg(package);

                        User.ID = Guid.NewGuid().ToString();
                        User.Login = handshake.Login;
                        User.Password = handshake.Password;
                        User.FriendID = null;
                        return true;
                    }
                    _logger.log("Client bad handshake!", this.GetType().Name);
                    return false;
                }
            }
            catch (IOException)
            {
                _logger.log("No Handshake Exeption or Timeout!", this.GetType().Name);
                return false;
            }
        }

        public async Task Run()
        {
            int ErrorCount = 0;
            DateTime LastMSGTime = DateTime.MinValue;

            _isConnected = Handshake();

            if (_isConnected)
            {
                _stream.ReadTimeout = Timeout.Infinite;
                _logger.log("Client Connected " + User.ID, this.GetType().Name);
                OnClientConnected?.Invoke(User.ID, this);
            }
            else 
            {
                Disconnect("No HandShake");
                return; 
            }
            
            try
            {
                while (_isConnected)
                {
                    byte[] buffer = new byte[MessagingConsts.MaxLength + MessagingConsts.MaxNameLength];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
                    {
                        ErrorCount++;
                        _logger.log(User.Login + ":To fast " + ErrorCount, this.GetType().Name);
                        if (ErrorCount > 2)
                        {
                            Disconnect("Flood");
                            return;
                        }
                    }
                    if (bytesRead <= 0)
                    {
                        Disconnect(User.Login + "Disconnects the connection");
                        return;
                    }
                    else
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        
                        //Later ¯\_(ツ)_/¯

                    }
                }
                Disconnect(null);
                return;
            }
            catch (Exception ex)
            {
                Disconnect(ex.Message);
                return;
            }
        }

        public async void Send(ChatMessage message)
        {
            byte[] msg = UnicodeEncoding.UTF8.GetBytes(message.ToString());
            await _stream.WriteAsync(msg, 0, msg.Length);
        }

        public void SendSystemMsg(ServerCodes code)
        {
            ChatMessage message = new ChatMessage();
            DateTime utcNow = DateTime.UtcNow;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            message.SendTime = utcNow - unixEpoch;
            message.Target = this.GetType().Name;
            message.Sender = "Server";
            message.Text = code.ToString();

            byte[] msg = Encoding.UTF8.GetBytes(message.ToString());
            _stream.Write(msg, 0, msg.Length);
        }

        public void SendSystemMsg(string message)
        {
            byte[] msg = Encoding.UTF8.GetBytes(message);
            _stream.Write(msg, 0, msg.Length);
        }

        public async void Disconnect(string? cause)
        {
            _isConnected = false;
            await _stream.DisposeAsync();
            _client.Dispose();
            if (cause != null) _logger.log($"Client {User.ID} Disconnected: {cause}", this.GetType().Name);
            else _logger.log($"Client {User.ID} Disconnected", this.GetType().Name);
            _storage.DeleteSession(User.ID);
            OnClientDead?.Invoke(User.ID);
        }
    }
}