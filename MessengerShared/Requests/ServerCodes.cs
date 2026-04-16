namespace MessengerShared.Requests
{
    public enum ServerCodes
    {
        NoErrors = 200, //All right, go ahead!
        Hello = 1, //Server is ready for handshake
        Disconnected = 2, //Touch grass bro, you are disconnected kekb
        BadRequest = 400, //Bad request format
        TooManyRequests = 429, //Too many messages, slow down!
        HandshakeFailed = 525, //OOPS, what's wrong again O_o?!
        AccessTokenExpired = 406, //Access token expired
        NoTargetUser = 404, //IDK who is it
        Unauthorized = 401, //User Unauthorized
        WrongPassword = 512, //You are not allowed to be authorized!
        TooManyErrors = 1024, //Too many errors, you are so fucking silly, disconnecting you ¯\_(ツ)_/¯
    }
}