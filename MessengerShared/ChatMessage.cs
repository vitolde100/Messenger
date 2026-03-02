
namespace MessengerShared
{
    public class ChatMessage
    {
        public TimeSpan SendTime { get; set; }
        public string Target { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }

        public ChatMessage() {   }

        public ChatMessage(TimeSpan sendTime, string target, string sender, string text)
        {
            SendTime = sendTime;
            Target = target;
            Sender = sender;
            Text = text;
        }

        /// <summary>
        /// Returns a string that represents the current object, including the send time, target, sender, and message.
        /// </summary>
        /// <returns> Returns a string containing a message in the format: {SendTime}|{Target}|{Sender}|{Text}.
        /// </returns>
        public override string ToString()
        {
            return $"{SendTime:c}|{Target}|{Sender}|{Text}";
        }

        public static bool TryParse(string msg, out ChatMessage message)
        {
            message = null;

            string[] data = msg.Split(MessagingConsts.SplitChar,MessagingConsts.PartsCount);

            if (data.Length != MessagingConsts.PartsCount) 
                return false;

            if (!TimeSpan.TryParse(data[0], out TimeSpan time))
                return false;

            if (string.IsNullOrEmpty(data[1]) ||
                string.IsNullOrEmpty(data[3]) ||
                data[2].Length > MessagingConsts.MaxNameLength ||
                data[3].Length > MessagingConsts.MaxLength)
                return false;

            message = new ChatMessage(time, data[1], data[2], data[3]);
            
            return true;
        }
    }
}