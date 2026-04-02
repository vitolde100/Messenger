namespace MessengerShared
{
    public enum ServerCodes
    {
        None = 0,
        HandshakeSuccess = 1,
        HandshakeFailed = 2,
        NoTargetClient = 3,
    }

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
        public const char SplitChar = '卐';
    }
}