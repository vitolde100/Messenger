using MessengerShared.API;

namespace MessengerClient.Client
{
    internal class State
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string IP { get; set; } = "192.168.1.2";
        public int Port { get; set; } = 5000;
        public string UserID { get; set; }
        public Session Session { get; set; }
        public bool isLoggedIn { get
            {
                return Session != null && !Session.IsAccessExpired();
            }
        }


        public State()
        {
            
        }

        public void Clear()
        {
            Login = null;
            Password = null;
            IP = null;
            Port = 0;
            UserID = null;
            Session = null;
        }   
    }
}