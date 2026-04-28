namespace MessengerServer.Data
{
    internal class ClientContext
    {
        public string UserID { get; set; }
        public string AccessToken { get; set; }
        public int? RegistryID { get; set; }
        public bool isAuthenticated { get =>  !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(UserID);}
        public ClientContext()
        {

        }
    }
}