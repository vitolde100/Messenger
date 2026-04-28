using MessengerServer.Core;
using MessengerServer.Data;
using MessengerServer.Services;
using MessengerShared.Requests;

namespace MessengerServer.Requests.Handlers
{
    internal class Logout : RequestHandler
    {
        SessionService _sessionService;
        ClientRegistry _clientRegistry;
        public Logout(SessionService sessionService, ClientRegistry registry)
        {
            _sessionService = sessionService;
            _clientRegistry = registry;
            ShouldBeAutorised = true;
        }

        public override Response Handle(Request request, ClientHandler client)
        {
            var userId = client.Context.UserID;

            client.Deauthenticate();

            _sessionService.Remove(request.AccessToken);

            if (userId != null)
                _clientRegistry.Remove(userId);

            return BuildResponce();
        }
    }
}
