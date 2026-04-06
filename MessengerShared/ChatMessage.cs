namespace MessengerShared
{
    public class ChatMessage
    {
        public TimeSpan SendTime { get; set; }
        public string TargetID { get; set; }
        public string AccessToken { get; set; }
        public string Text { get; set; }

        public ChatMessage() {   }

        public ChatMessage(string Token, TimeSpan sendTime, string target, string text)
        {
            AccessToken = Token;
            SendTime = sendTime;
            TargetID = target;
            Text = text;
        }

        /// <summary>
        /// Returns a string that represents the current object, including the send time, target, sender, and message.
        /// </summary>
        /// <returns> Returns a string containing a message in the format: {accessToken}|{SendTime}|{TargetID}|{Text}.
        /// </returns>
        public override string ToString()
        {
            return $"{AccessToken}|{SendTime:c}|{TargetID}|{Text}";
        }

        public static bool TryParse(string msg, out ChatMessage message)
        {
            message = null;

            string[] data = msg.Split(MessagingConsts.SplitChar,MessagingConsts.PartsCount);

            if (data.Length != MessagingConsts.PartsCount) 
                return false;

            if (string.IsNullOrEmpty(data[0]) ||
                !TimeSpan.TryParse(data[1], out TimeSpan time) ||
                string.IsNullOrEmpty(data[2]) ||
                data[3].Length > MessagingConsts.MaxLength)
                return false;

            message = new ChatMessage(data[0], time, data[2], data[3]);
            
            return true;
        }
    }
}