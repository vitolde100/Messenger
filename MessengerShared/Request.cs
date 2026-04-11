namespace MessengerShared
{
    internal class Request
    {
        string AccessToken;
        string RefreshToken;
        string Type;
        string Data;

        public Request(string token, string type, string data)
        {
            AccessToken = token;
            Type = type;
            Data = data;
        }
    }
}
