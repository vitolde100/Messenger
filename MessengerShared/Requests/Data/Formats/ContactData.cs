using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data.Formats
{
    public class ContactData
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string userId { get; set; }
    }
}
