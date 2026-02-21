namespace MessengerShared
{
    public static class Protocol
    {
        public const string Version = "1.0";
        public const int DefaultPort = 5000;
    }

    public static class MessagingConsts
    {
        public const int MaxNameLength = 32;
        public const int MaxLength = 4096;
        public const int PartsCount = 4;
        public const char SplitChar = '|';
    }
}