using MessengerServer.Core;
using MessengerServer.Requests.Handlers;
using MessengerServer.Services;
using MessengerShared.Requests;
using System.Text.Json;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRouter
    {
        private Dictionary<string, IRequestHandler> _handlers = new Dictionary<string, IRequestHandler>();

        private SessionService _sessionService;
        private IRequestHandler _logoutHandler;
        private Logger _logger = Logger.instance;

        public RequestRouter(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public void RegisterHandler(IRequestHandler handler)
        {
            _handlers[handler.Type] = handler;
            if (handler.Type == "Logout") _logoutHandler = handler;
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
                var session = _sessionService.GetSessionByAccessToken(request.AccessToken);

                if (handler.ShouldBeAutorised && handler.Type != "Refresh")
                {
                    if (!client.Context.isAuthenticated)
                    {
                        _logger.log($"User Unathorised: {request.AccessToken}", GetType().Name);
                        return BuildResponce(request, ServerCodes.Unauthorized);
                    }

                    if(session == null)
                    {
                        _logger.log($"Invalid access token: {request.AccessToken}", GetType().Name);
                        _logoutHandler.HandleRequest(request, client);
                        return BuildResponce(request, ServerCodes.SessionNotExist);
                    }

                    if(session.IsRefreshExpired())
                    {
                        _logger.log($"SessionExpired: {request.AccessToken}", GetType().Name);
                        _logoutHandler.HandleRequest(request, client);
                        return BuildResponce(request, ServerCodes.SessionExpired);
                    }
                    
                    if(session.IsAccessExpired())
                    {
                        _logger.log($"AccessTokenExpired: {request.AccessToken}", GetType().Name);
                        return BuildResponce(request, ServerCodes.AccessTokenExpired);
                    }

                    responce = handler.HandleRequest(request, client);
                }
                else responce = handler.HandleRequest(request, client);
                
                responce.Number = request.Number;
                return responce;
            }
            else
            {
                _logger.log($"No handler found for request type: {request.Type}", GetType().Name);
                return new Response
                {
                    Number = request.Number,
                    Type = request.Type,
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
                Type = request.Type,
                Error = code,
                Success = false,
                Data = null
            };
            return responce;
        }
    }
}
 