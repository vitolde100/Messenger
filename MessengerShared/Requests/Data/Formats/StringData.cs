using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data.Formats
{
    public class StringData
    {
        [Required]
        public string StringStr { get; set; }
    }
}
