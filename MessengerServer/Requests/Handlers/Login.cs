using MessengerServer.Services;
using MessengerServer.Core;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;
using MessengerServer.Data;

namespace MessengerServer.Requests.Handlers
{
    internal class Login : IRequestHandler
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

        public override Responce HandleRequest(Request request, ClientHandler client)
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
