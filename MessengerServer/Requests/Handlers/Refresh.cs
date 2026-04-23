using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Refresh : IRequestHandler
    {
        private SessionService _sessionService;
        public Refresh(SessionService seService)
        {
            _sessionService = seService;

            ShouldBeAutorised = false;
        }

        public override Response HandleRequest(Request request, ClientHandler client)
        {
            if (request.AccessToken == null) return BuildResponce(ServerCodes.Unauthorized);

            var Data = JsonSerializer.Deserialize<StringData>(JsonSerializer.Serialize(request.Data));
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            var session = _sessionService.GetSessionByAccessToken(request.AccessToken);
            if (session == null) return BuildResponce(ServerCodes.NoTargetSession);

            if(session.refreshToken != Data.StringStr) return BuildResponce(ServerCodes.WrongRefreshToken);

            _sessionService.Remove(session.accessToken);

            return BuildResponce(_sessionService.CreateSession(session.userID));
        }
    }
}
