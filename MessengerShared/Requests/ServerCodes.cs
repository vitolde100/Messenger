namespace MessengerShared.Requests
{
    public enum ServerCodes
    {
        NoErrors = 0, //All right, go ahead!
        Hello = 1, //Server is ready for handshake
        Disconnected = 2, //Touch grass bro, you are disconnected kekb
        
        BadRequest = 400, //Bad request format
        TooManyRequests = 429, //Too many messages, slow down!
        TooManyErrors = 1024, //Too many errors, you are so fucking silly, disconnecting you ¯\_(ツ)_/¯

        AccessTokenExpired = 406, //Access token expired
        SessionExpired = 408, //Session expired
        SessionNotExist = 407, //Session not exist
        Unauthorized = 401, //User Unauthorized

        WrongPassword = 512, //You are not allowed to be authorized!
        WrongAccessToken = 511, //You are not allowed to access this resource!
        WrongRefreshToken = 513, //You are not allowed to refresh your session!

        NoTargetUser = 404, //IDK who is it
        NoTargetSession = 405, //IDK what session is it


        ClientAlreadyExist = 674,
    }
}