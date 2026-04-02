namespace MessengerShared
{
    public class HandshakeMessage
    {
        public bool Status {  get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public HandshakeMessage() { }

        public HandshakeMessage(bool status, string login, string password)
        { 
            Status = status;
            Login = login;
            Password = password;
        }

        /// <summary>
        /// Returns a string that represents the current object, including the Handshake status, Login, Password.
        /// </summary>
        /// <returns> Returns a string containing a message in the format: {Status}|{Login}|{Password}.
        /// </returns>
        public override string ToString()
        {
            string status;
            if (Status) status = "reg";
            else status = "log";
            return $"{status}|{Login}|{Password}";
        }

        public static bool TryParse(string msg, out HandshakeMessage message)
        {
            message = null;

            string[] data = msg.Split(MessagingConsts.SplitChar, MessagingConsts.HandshakeCount);

            if (data.Length != MessagingConsts.HandshakeCount)
                return false;

            if (string.IsNullOrEmpty(data[0]) ||
                data[1].Length > MessagingConsts.MaxNameLength ||
                string.IsNullOrEmpty(data[2]))
                return false;

            bool status;
            if (data[0] == "reg") { status = false; }
            else if (data[1] == "log") { status = true; } //Костыль, нужен enum для статуса, но не хочется его делать ради 2х значений.
            else return false;

            message = new HandshakeMessage(status, data[1], data[2]);

            return true;
        }
    }
}