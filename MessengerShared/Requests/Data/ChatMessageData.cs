using System.ComponentModel.DataAnnotations;

namespace MessengerShared.Requests.Data
{
    public class ChatMessageData
    {
        [Required]
        public TimeSpan SendTime { get; set; }
        [Required]
        public string TargetID { get; set; }
        [Required]
        public string Text { get; set; }
        public ChatMessageData() { }

        public ChatMessageData(string targetID, string text) 
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            SendTime = utcNow - unixEpoch;
            TargetID = targetID;
            Text = text;
        }
    }
}