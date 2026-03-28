using MessengerShared;
using System.Net.Sockets;
using System.Text;

namespace MessengerServer
{
    internal class ClientHandler
    {
        bool _isConnected;
        public string UserID { get; set; }
        public string Name { get; private set; }
        TcpClient _client;
        Stream _stream;
        Logger _logger = Logger.instance;

        public event Action<string, ClientHandler> OnClientConnected;
        public event Action<ClientHandler, ChatMessage> OnMessageRecieved;
        public event Action<string> OnClientDead;

        TimeSpan MSGCooldown = TimeSpan.FromSeconds(1.5f);

        public ClientHandler(TcpClient client, Stream stream)
        {
            _client = client;
            _stream = stream;
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
                    _logger.log("No Handhake", this.GetType().Name);
                    return false;
                }
                else
                {
                    Name = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (!string.IsNullOrEmpty(Name) && Name.Length <= MessagingConsts.MaxNameLength)
                    {
                        return true;
                    }
                    _logger.log("Client bad name:" + Name.Length, this.GetType().Name);
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
                _logger.log("Client Connected " + Name, this.GetType().Name);
                _stream.ReadTimeout = Timeout.Infinite;

                OnClientConnected?.Invoke(Name, this);
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
                        _logger.log(Name + ":To fast " + ErrorCount, this.GetType().Name);
                        if (ErrorCount > 2)
                        {
                            Disconnect("Flood");
                            return;
                        }
                    }
                    if (bytesRead <= 0)
                    {
                        Disconnect(Name + "Disconnects the connection");
                        return;
                    }
                    else
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        ChatMessage? message;
                        
                        if (ChatMessage.TryParse(msg, out message))
                        {
                            OnMessageRecieved?.Invoke(this, message);
                            LastMSGTime = DateTime.UtcNow;
                        }
                        else
                        {
                            Disconnect("Bad Message " + msg);
                            return;
                        }
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

        public void SendSystemMsg(string text)
        {
            ChatMessage message = new ChatMessage();
            DateTime utcNow = DateTime.UtcNow;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            message.SendTime = utcNow - unixEpoch;
            message.Target = this.GetType().Name;
            message.Sender = this.GetType().Name;
            message.Text = text;

            byte[] msg = Encoding.UTF8.GetBytes(message.ToString());
            _stream.Write(msg, 0, msg.Length);
        }

        public async void Disconnect(string? cause)
        {
            _isConnected = false;
            await _stream.DisposeAsync();
            _client.Dispose();
            if (cause != null) _logger.log($"Client {Name} Disconnected: {cause}", this.GetType().Name);
            else _logger.log($"Client {Name} Disconnected", this.GetType().Name);

            OnClientDead?.Invoke(Name);
        }
    }
}