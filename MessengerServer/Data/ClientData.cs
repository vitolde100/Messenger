namespace MessengerServer.Data
{
    public class ClientData
    {
        
        public string ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } //По факту hash
        public string FriendID { get; set; } //ДАДАДА КОГДА-НИБУДЬ БУДЕТ (⊙_⊙;)

        public ClientData() { }
    }
}