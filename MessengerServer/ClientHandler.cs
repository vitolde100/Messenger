using MessengerServer.Data;
using MessengerShared;
using MessengerShared.API;
using Microsoft.Data.Sqlite;
using System.Diagnostics.Contracts;
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
        IStorage _storage;

        bool _isConnected;
        public ClientData User = new ClientData();

        public event Action<string, ClientHandler> OnClientConnected;
        public event Action<ClientHandler, ChatMessage> OnMessageRecieved;
        public event Action<string> OnClientDead;

        TimeSpan MSGCooldown = TimeSpan.FromSeconds(0f); // DO NOT FORGET TO SET

        DateTime LastMSGTime = DateTime.MinValue;
        const int MaxErrorCount = 10;
        int ErrorCount = 0;

        public ClientHandler(TcpClient client, Stream stream, IStorage repository)
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
                SendSystemMsg(ServerCodes.Hello);
                _stream.ReadTimeout = 10000;
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
                        User.ID = Guid.NewGuid().ToString();
                        User.Login = handshake.Login;
                        User.Password = handshake.Password;
                        User.FriendID = null;

                        while (true)
                            try
                            {
                                _storage.SaveClient(User);
                                break;
                            }
                            catch (SqliteException)
                            {
                                User.ID = Guid.NewGuid().ToString();
                            }
                        
                        _storage.SaveClient(User);

                        string accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                        string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

                        var session = new Session(accessToken, refreshToken, User.ID);

                        while (true)
                            try
                            {
                                _storage.SaveSession(session);
                                break;
                            }
                            catch (SqliteException)
                            {
                                accessToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                                refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                            }

                        string package = session.ConvertToPackage();
                        SendSystemMsg(package);

                        return true;
                    }
                    _logger.log("Client bad handshake!", this.GetType().Name);
                    return false;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        public async Task Run()
        {
            _isConnected = Handshake();

            if (_isConnected)
            {
                _stream.ReadTimeout = Timeout.Infinite;
                _logger.log("Client Connected " + User.ID, this.GetType().Name);
                OnClientConnected?.Invoke(User.ID, this);
            }
            else 
            {
                Disconnect(ServerCodes.HandshakeFailed);
                return; 
            }
            
            try
            {
                while (_isConnected)
                {
                    if (ErrorCount > MaxErrorCount)
                    {
                        Disconnect(ServerCodes.TooManyErrors);
                        return;
                    }

                    byte[] buffer = new byte[MessagingConsts.MaxLength + MessagingConsts.MaxNameLength];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        Disconnect(null);
                        return;
                    }
                    else
                    {

                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string reqCode = msg.Split(MessagingConsts.SplitChar)[0];

                        ProcessMessage(msg);
                    }
                }
                Disconnect(null);
                return;
            }
            catch (Exception ex)
            {
                Disconnect(null, ex.Message);
                return;
            }
        }

        public async void ProcessMessage(string msg)
        {
            try
            {
                ChatMessage message = new ChatMessage();

                if (DateTime.UtcNow - LastMSGTime < MSGCooldown)
                {
                    ErrorCount++;
                    SendSystemMsg(ServerCodes.TooManyRequests);
                    _logger.log(User.Login + ":TooManyRequests " + ErrorCount, this.GetType().Name);
                }

                if (!ChatMessage.TryParse(msg, out message))
                {
                    SendSystemMsg(ServerCodes.BadRequest);
                    _logger.log("Client " + User.Login + " sent bad Request!", this.GetType().Name);
                    ErrorCount++;
                }

                var session = _storage.GetSessionByAccessToken(message.AccessToken);

                if (session == null && session.userID != User.ID)
                {
                    SendSystemMsg(ServerCodes.Unauthorized);
                    _logger.log("Client " + User.Login + " now unauthorized!", this.GetType().Name);
                    //Login()
                    
                }

                if (!session.isAccessValid())
                {
                    SendSystemMsg(ServerCodes.AccessTokenExpired);
                    _logger.log("Client " + User.Login + " AccessToken expired!", this.GetType().Name);
                    //Wait RefreshToken or Login()
                }

                OnMessageRecieved?.Invoke(this, message);
                LastMSGTime = DateTime.UtcNow;


            }
            catch (Exception ex)
            {
                _logger.log("Error while sending message: " + ex.Message, this.GetType().Name);
            }
        }

        public async void Send(ChatMessage message)
        {
            Send(message.ToString());
        }

        public async void Send(string message)
        {
            byte[] msg = UnicodeEncoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(msg, 0, msg.Length);
        }

        public void SendSystemMsg(ServerCodes? code)
        {
            try
            {
                Send(code.ToString());
            }
            catch (Exception ex)
            {
                _logger.log("Error while sending system message: " + ex.Message, this.GetType().Name);
            }
        }

        public void SendSystemMsg(string message)
        {
            try
            {
                Send(message);
            }
            catch (Exception ex)
            {
                _logger.log("Error while sending system message: " + ex.Message, this.GetType().Name);
            }
        }

        public async void Disconnect(ServerCodes? code, string? Ex = null)
        {
            if (code != null) SendSystemMsg(code);
            else SendSystemMsg(ServerCodes.Disconnected);

            _isConnected = false;
            await _stream.DisposeAsync();
            _client.Dispose();

            if (Ex != null) _logger.log($"Client {User.ID} disconnected with error: {Ex}", this.GetType().Name);
            else _logger.log($"Client {User.ID} Disconnected", this.GetType().Name);

            OnClientDead?.Invoke(User.ID);
        }
    }
}