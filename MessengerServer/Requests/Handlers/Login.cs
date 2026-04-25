using MessengerServer.Services;
using MessengerServer.Core;
using MessengerShared.Requests;
using System.Text.Json;
using MessengerServer.Data;
using MessengerShared.Requests.Enums;

namespace MessengerServer.Requests.Handlers
{
    internal class Login : RequestHandler
    {
        SessionService _sessionService;
        ClientService _clientService;
        ClientRegistry _clientRegistry;
        public Login(SessionService seService, ClientService clService, ClientRegistry registry) : base()
        {
            _sessionService = seService;
            _clientService = clService;
            _clientRegistry = registry;

            ShouldBeAutorised = false;
        }

        public override Response Handle(Request request, ClientHandler client)
        {
            var Data = JsonSerializer.Deserialize<ClientData>(JsonSerializer.Serialize(request.Data));
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            var User = _clientService.GetClientByLogin(Data.Login);

            if (User == null)
                return BuildResponce(ServerCodes.NoTargetUser);
            
            if (!BCrypt.Net.BCrypt.Verify(Data.Password, User.Password))
                return BuildResponce(ServerCodes.WrongPassword);

            var session = _sessionService.CreateSession(User.ID);

            client.Context.UserID = User.ID;
            client.Context.AccessToken = session.accessToken;
            _clientRegistry.Add(client);

            return BuildResponce(session);
        }
    }
}
