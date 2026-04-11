using System.Text.Json;

namespace MessengerShared
{
    public class ChatMessageData
    {
        public TimeSpan SendTime;
        public string TargetID;
        public string Text;

        public ChatMessageData() {   }

        public ChatMessageData(TimeSpan sendTime, string target, string text)
        { 
            SendTime = sendTime;
            TargetID = target;
            Text = text;
        }

        public static bool TryParse(string Req, out ChatMessageData Data)
        {
            Data = null;
            Data = JsonSerializer.Deserialize<ChatMessageData>(Req);

            if (Data == null) return false;
            if (Data.SendTime == TimeSpan.Zero) return false; //Кек, прикинь, если время отправки не указано, то оно будет равно TimeSpan.Zero, а не null...
            if (Data.TargetID == null) return false;
            if (Data.Text == null) return false;
            return true;
        }
    }
}