using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data.Chats
{
    public class AddToChatData
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string GroupId { get; set; }
    }
}
