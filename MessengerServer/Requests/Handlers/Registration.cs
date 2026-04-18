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

        public override Responce HandleRequest(Request request, ClientContext context)
        {
            var Data = JsonSerializer.Deserialize<UserData>((JsonElement)request.Data);
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            var User = _clientService.CreateClient(Data.Login, Data.Password);
            var session = _sessionService.CreateSession(User.ID);

            context.UserID = User.ID;

            return BuildResponce(session.ConvertToElement());
        }
    }
}
