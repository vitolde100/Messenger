using MessengerServer.Core;
using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Enums;

namespace MessengerServer.Requests.Handlers
{
    internal class Hello : RequestHandler
    {
        private SessionService _sessionService;
        private ClientRegistry _clientRegistry;
        
        public Hello(SessionService sessionService, ClientRegistry clientRegistry) 
        {
            _sessionService = sessionService;
            _clientRegistry = clientRegistry;

            ShouldBeAutorised = false;
        }
        
        public override Response Handle(Request requests, ClientHandler handler)
        {
            if (handler.Context.isAuthenticated) return BuildResponce(ServerCodes.AlreadyAuthorised);
            var session = _sessionService.GetSessionByAccessToken(requests.AccessToken);
                
            if (session == null) return BuildResponce(ServerCodes.SessionNotExist);
            handler.Context.UserID = session.userID;
            handler.Context.AccessToken = requests.AccessToken;
            _clientRegistry.Add(handler);

            return BuildResponce();
        }
    }
}
