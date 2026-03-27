namespace MessengerShared
{
    public class ChatUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }

        public ChatUser(string UID) 
        {
            UserID = UID;
        }
    }
}
