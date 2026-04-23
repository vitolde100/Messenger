using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data
{
    public class StringData
    {
        [Required]
        public string StringStr { get; set; }
    }
}
