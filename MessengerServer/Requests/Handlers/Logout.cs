using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Logout : IRequestHandler
    {
        SessionService _sessionService;
        public Logout(SessionService sessionService)
        {
            _sessionService = sessionService;
            ShouldBeAutorised = true;
        }

        public override Responce HandleRequest(JsonElement json, ClientContext context)
        {
            var Data = JsonSerializer.Deserialize<LogoutData>(json);
            if (Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            _sessionService.RemoveSession(Data.AccessToken);
            
            return BuildResponce();
        }
    }
}
