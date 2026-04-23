using MessengerServer.Core;
using MessengerServer.Services;
using MessengerShared.Requests;

namespace MessengerServer.Requests.Handlers
{
    internal class Logout : IRequestHandler
    {
        SessionService _sessionService;
        ClientRegistry _clientRegistry;
        public Logout(SessionService sessionService, ClientRegistry registry)
        {
            _sessionService = sessionService;
            _clientRegistry = registry;
            ShouldBeAutorised = true;
        }

        public override Response HandleRequest(Request request, ClientHandler client)
        {
            client.Deauthenticate();

            _sessionService.Remove(request.AccessToken);
            _clientRegistry.Remove(client.Context);

            return BuildResponce();
        }
    }
}
