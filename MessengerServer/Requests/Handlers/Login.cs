using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Login : IRequestHandler
    {
        SessionService _sessionService;
        ClientService _clientService;
        public Login(SessionService seService, ClientService clService) : base()
        {
            _sessionService = seService;
            _clientService = clService;

            ShouldBeAutorised = false;
        }

        public override Responce HandleRequest(Request request, ClientContext context)
        {
            var Data = JsonSerializer.Deserialize<UserData>((JsonElement)request.Data);
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            var User = _clientService.GetClientByLogin(Data.Login);

            if (User == null)
                return BuildResponce(ServerCodes.NoTargetUser);
            
            if (!BCrypt.Net.BCrypt.Verify(Data.Password, User.Password))
                return BuildResponce(ServerCodes.WrongPassword);

            var session = _sessionService.CreateSession(User.ID);

            context.UserID = User.ID;
            context.AccessToken = session.accessToken;

            return BuildResponce(session.ConvertToElement());
        }
    }
}
