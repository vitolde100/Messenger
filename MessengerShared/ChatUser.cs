namespace MessengerShared
{
    public class ChatUser
    {
        public string UserID { get; set; }
        public string FriendID { get; set; }
        public string SessionID { get; set; }
        public string UserName { get; set; }

        public ChatUser(string UUID) 
        {
            UserID = UUID;
        }
    }
}
