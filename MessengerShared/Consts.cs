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
        public const int PartsCount = 4;
        public const int HandshakeCount = 3;
        public const char SplitChar = '|';
    }
}