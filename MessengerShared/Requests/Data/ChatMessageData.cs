using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data
{
    public class ChatMessageData
    {
        [Required]
        public TimeSpan SendTime;
        [Required]
        public string TargetID;
        [Required]
        public string Text;

        public ChatMessageData() {   }

        public ChatMessageData(TimeSpan sendTime, string target, string text)
        { 
            SendTime = sendTime;
            TargetID = target;
            Text = text;
        }
    }
}