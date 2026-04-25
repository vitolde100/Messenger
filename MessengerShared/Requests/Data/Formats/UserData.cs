using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data.Formats
{
    public class UserData
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
