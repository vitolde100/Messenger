namespace MessengerServer.Requests
{
    internal class ClientContext
    {
        public string UserID { get; set; }
        public string AccessToken { get; set; }
        public int? RegistryID { get; set; }
        public bool isAuthenticated { get => RegistryID != default && !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(UserID);}
        public ClientContext()
        {

        }
    }
}