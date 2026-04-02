namespace MessengerServer.Data
{
    public class ClientData
    {
        public string ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } //Потом hash
        public string FriendID { get; set; }
        public string SessionID { get; set; }

        public ClientData() { }
    }
}