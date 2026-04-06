namespace MessengerShared
{
    public enum ServerCodes
    {
        NoErrors = 200, //All right, go ahead!
        Hello = 1, //Server is ready for handshake
        Disconnected = 2, //Touch grass bro, you are disconnected XD
        BadRequest = 400, //Bad message format
        TooManyRequests = 429, //Too many messages, slow down
        HandshakeFailed = 525, //Handshake failed
        AccessTokenExpired = 406, //Access token expired
        NoTargetClient = 404, //Wrong TargetID
        Unauthorized = 401, //User Unauthorized
        TooManyErrors = 1024, //Too many errors, you are so fucking silly, disconnecting you ¯\_(ツ)_/¯
    }
}