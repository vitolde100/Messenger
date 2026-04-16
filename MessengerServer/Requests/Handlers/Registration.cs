using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Registration : IRequestHandler
    {
        SessionService _sessionService;
        ClientService _clientService;
        public Registration(SessionService seService, ClientService clService) : base()
        { 
            _sessionService = seService;
            _clientService = clService;

            ShouldBeAutorised = false;
        }

        public override Responce HandleRequest(JsonElement json, ClientContext context)
        {
            var Data = JsonSerializer.Deserialize<UserData>(json);
            if (!Validate(json)) return BuildResponce(ServerCodes.BadRequest);

            var User = _clientService.CreateClient(Data.Login, Data.Password);
            var session = _sessionService.CreateSession(User.ID);

            context.UserID = User.ID;

            return BuildResponce(session.ConvertToElement());
        }
    }
}
