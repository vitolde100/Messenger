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
            var data = JsonSerializer.Deserialize<ClientData>(request.Data.ToString());

            if (!Validate(data))
                return BuildResponce(ServerCodes.BadRequest);

            if (_clientService.GetClientByLogin(data.Login) != null)
                return BuildResponce(ServerCodes.ClientAlreadyExist);

            var user = _clientService.CreateClient(data.Login, data.Password);
            var session = _sessionService.CreateSession(user.ID);

            client.Context.UserID = user.ID;
            client.Context.AccessToken = session.accessToken;

            _clientRegistry.Add(user.ID,client);

            return BuildResponce(session);
        }
    }
}
