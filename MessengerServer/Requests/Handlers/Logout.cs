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

        public override Responce HandleRequest(Request request, ClientContext context)
        {
            _sessionService.Remove(request.AccessToken);
            _clientRegistry.Remove(context);

            return BuildResponce();
        }
    }
}
