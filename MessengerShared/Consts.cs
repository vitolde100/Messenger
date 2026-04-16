namespace MessengerShared
{
    public static class Protocol
    {
        public const int DefaultPort = 5000;
    }

    public static class MessagingConsts
    {
        public const int MaxNameLength = 32;
        public const int MaxLength = 4096;

        //Forget it, it shouldn't exist later!
        //Only requests and responses are in JSON format,
        //but I'll leave it here for now,
        //I don't want too many errors.
        /*
        public const int PartsCount = 4; 
        public const int HandshakeCount = 3;
        public const char SplitChar = '|'; 
        */
    }
}