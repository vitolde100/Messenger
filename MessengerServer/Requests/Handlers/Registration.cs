using MessengerServer.Core;
using MessengerServer.Data;
using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Enums;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Registration : RequestHandler
    {
        SessionService _sessionService;
        ClientService _clientService;
        ClientRegistry _clientRegistry;
        public Registration(SessionService seService, ClientService clService, ClientRegistry registry) : base()
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

            if (_clientService.GetClientByLogin(Data.Login) != null) return BuildResponce(ServerCodes.ClientAlreadyExist);
            var User = _clientService.CreateClient(Data.Login, Data.Password);
            var session = _sessionService.CreateSession(User.ID);

            client.Context.UserID = User.ID;
            client.Context.AccessToken = session.accessToken;
            _clientRegistry.Add(client);

            return BuildResponce(session);
        }
    }
}
