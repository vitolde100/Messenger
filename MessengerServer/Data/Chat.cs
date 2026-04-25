using MessengerServer.Core;
using MessengerServer.Requests.Handlers;
using MessengerShared.Requests.Data;

namespace MessengerServer.Data
{
    internal class Chat
    {
        public bool isPersonal;
        public string Name { get; set; }
        public string Id { get; set; }
        private List<string> _users { get; set; }
        public List<ChatMessageData> Messages { get; set; }
        private Logger _logger = Logger.instance;

        public Chat() { }

        public Chat(bool ispersonal, string name)
        {
            isPersonal = ispersonal;
            Name = name;
        }

        public void AddUser(string ID)
        {
            if (!_users.Contains(ID))
            {
                _users.Add(ID);
            }
            _logger.log($"User added {ID} to {Name}", GetType().Name);
        }

        public List<string> GetUsers() { return _users; }

        public void RemoveUser(string ID)
        {
            try
            {
                _users.Remove(ID);

                _logger.log("Deleted " + ID + "\r", this.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.log("Can't Delete: " + ex.Message, this.GetType().Name);
            }
            _users.Remove(ID);
        }

        public void AddMessage(ChatMessageData message) { Messages.Add(message); }
    }
}
