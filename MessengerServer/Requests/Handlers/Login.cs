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
            var data = JsonSerializer.Deserialize<ClientData>(request.Data.ToString());

            if (!Validate(data))
                return BuildResponce(ServerCodes.BadRequest);

            var user = _clientService.GetClientByLogin(data.Login);

            if (user == null)
                return BuildResponce(ServerCodes.NoTargetUser);

            if (!BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
                return BuildResponce(ServerCodes.WrongPassword);

            var session = _sessionService.CreateSession(user.ID);

            // привязка сокета
            client.Context.UserID = user.ID;
            client.Context.AccessToken = session.accessToken;

            _clientRegistry.Add(user.ID,client);

            return BuildResponce(session);
        }
    }
}
