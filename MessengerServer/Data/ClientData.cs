using System.ComponentModel.DataAnnotations;

namespace MessengerServer.Data
{
    public class ClientData
    {
        
        public string ID { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; } //Потом hash
        public string FriendID { get; set; }

        public ClientData() { }
    }
}