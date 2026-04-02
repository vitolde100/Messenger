//Don`t look on this class I think, I remove it later or smth.

using System.Text.Json;

namespace MessengerShared
{
    public class HandshakeMessage
    {
        public class UserCredentials
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public double reqFlag;
        public string Keyword;
        public string Login;
        public string Password;
        public HandshakeMessage() { }

        public HandshakeMessage(string status, string keyword, string login, string password)
        { 
            reqFlag = Convert.ToDouble(status);
            Keyword = keyword;
            Login = login;
            Password = password;
        }

        /// <summary>
        /// Returns a string that represents the current object, including the Handshake reqFlag, Keyword, Login:Password.
        /// </summary>
        /// <returns> Returns a string containing a message in the format: {reqFlag}|{Keyword}|{Login:Password}.
        /// </returns>
        public override string ToString()
        {
            var Data = JsonSerializer.Serialize(new { l = Login, p = Password });
            return $"{reqFlag}|{Keyword}|{Data}";
        }

        public static bool TryParse(string msg, out HandshakeMessage message)
        {
            message = null;

            string[] data = msg.Split(MessagingConsts.SplitChar, MessagingConsts.HandshakeCount);

            if (data.Length != MessagingConsts.HandshakeCount)
                return false;

            if (string.IsNullOrEmpty(data[0]) ||
                data[1] != "auth" ||
                string.IsNullOrEmpty(data[2]))
                return false;
            var smth = JsonSerializer.Deserialize<UserCredentials>(data[2]);
            message = new HandshakeMessage(data[0], data[1], smth.Login, smth.Password);

            return true;
        }
    }
}