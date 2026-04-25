using MessengerServer.Core;
using MessengerServer.Requests.Handlers;
using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Enums;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRouter
    {
        private Dictionary<string, RequestHandler> _handlers = new Dictionary<string, RequestHandler>();

        private SessionService _sessionService;
        private RequestHandler _logoutHandler;
        private Logger _logger = Logger.instance;

        public RequestRouter(SessionService sessionService)
        {
            _sessionService = sessionService;
            _logger.log($"Request Router Initialized", GetType().Name);
        }

        public void RegisterHandler(RequestHandler handler)
        {
            _handlers[handler.Type] = handler;
        }

        public Response ProcessRequest(Request request, ClientHandler client)
        {
            if (request.AccessToken != client.Context.AccessToken && client.Context.isAuthenticated)
            {
                client.Deauthenticate();
                return BuildResponce(request, ServerCodes.Unauthorized);
            }

            if (_handlers.TryGetValue(request.Type, out var handler))
            {
                var responce = new Response();

                if (handler.ShouldBeAutorised && handler.Type != "Refresh")
                {
                    var session = _sessionService.GetSessionByAccessToken(request.AccessToken);
                    
                    if (!client.Context.isAuthenticated)
                    {
                        _logger.log($"User Unathorised: {request.AccessToken}", GetType().Name);
                        return BuildResponce(request, ServerCodes.Unauthorized);
                    }

                    if(session == null)
                    {
                        _logger.log($"Invalid access token: {request.AccessToken}", GetType().Name);
                        client.Deauthenticate();
                        return BuildResponce(request, ServerCodes.SessionNotExist);
                    }

                    if(_sessionService.isSessionRefreshValid(session.accessToken))
                    {
                        _logger.log($"SessionExpired: {request.AccessToken}", GetType().Name);
                        client.Deauthenticate();
                        return BuildResponce(request, ServerCodes.SessionExpired);
                    }
                    
                    if(_sessionService.isSessionAccessValid(session.accessToken))
                    {
                        _logger.log($"AccessTokenExpired: {request.AccessToken}", GetType().Name);
                        return BuildResponce(request, ServerCodes.AccessTokenExpired);
                    }

                    responce = handler.Handle(request, client);
                }
                else responce = handler.Handle(request, client);
                
                responce.Number = request.Number;
                return responce;
            }
            else
            {
                _logger.log($"No handler found for request type: {request.Type}", GetType().Name);
                return new Response
                {
                    Number = request.Number,
                    RequestType = request.Type,
                    Success = false,
                    Error = ServerCodes.BadRequest,
                    Data = null
                };
            }
        }

        private Response BuildResponce(Request request, ServerCodes code = ServerCodes.Unauthorized)
        {
            var responce = new Response
            {
                Number = request.Number,
                RequestType = request.Type,
                Error = code,
                Success = false,
                Data = null
            };
            return responce;
        }
    }
}
 