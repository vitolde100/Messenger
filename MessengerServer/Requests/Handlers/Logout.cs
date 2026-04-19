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
            ShouldBeAutorised = false;
        }

        public override Responce HandleRequest(Request request, ClientHandler client)
        {
            _sessionService.Remove(request.AccessToken);
            _clientRegistry.Remove(client.Context);

            return BuildResponce();
        }
    }
}
