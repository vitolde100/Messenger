using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data
{
    public class CreateChatData
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool isPersonal { get; set; }
        public string? ChatID { get; set; }

        public CreateChatData() { }

        public CreateChatData(bool ispersonal, string name )
        {
            Name = name;
            isPersonal = ispersonal;
        }
    }
}
