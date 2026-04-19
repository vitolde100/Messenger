using MessengerServer.Core;
using MessengerServer.Requests;
using MessengerServer.Requests.Handlers;
using MessengerServer.Services;
using MessengerShared.Requests;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRouter
    {
        private Dictionary<string, IRequestHandler> _handlers = new Dictionary<string, IRequestHandler>();

        private SessionService _sessionService;
        private Logger _logger = Logger.instance;

        public RequestRouter(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public void RegisterHandler(IRequestHandler handler)
        {
            _handlers[handler.Type] = handler;
        }

        public Responce ProcessRequest(Request request, ClientHandler client)
        {
            if (_handlers.TryGetValue(request.Type, out var handler))
            {
                var responce = new Responce();
                var session = _sessionService.GetSessionByAccessToken(request.AccessToken);

                if (handler.ShouldBeAutorised && handler.Type != "Refresh")
                {
                    if(session == null)
                    {
                        _logger.log($"Invalid access token: {request.AccessToken}", GetType().Name);
                        return new Responce
                        {
                            Number = request.Number,
                            Type = request.Type,
                            Success = false,
                            Error = ServerCodes.SessionNotExist,
                            Data = JsonDocument.Parse("{}").RootElement
                        };
                    }

                    if(session.IsRefreshExpired())
                    {
                        _logger.log($"SessionExpired: {request.AccessToken}", GetType().Name);
                        _handlers.TryGetValue("Logout", out var logoutHandler);
                        logoutHandler.HandleRequest(request, client);
                        return new Responce
                        {
                            Number = request.Number,
                            Type = request.Type,
                            Success = false,
                            Error = ServerCodes.SessionExpired,
                            Data = JsonDocument.Parse("{}").RootElement
                        };
                    }
                    
                    if(session.IsAccessExpired())
                    {
                        _logger.log($"AccessTokenExpired: {request.AccessToken}", GetType().Name);
                        return new Responce
                        {
                            Number = request.Number,
                            Type = request.Type,
                            Success = false,
                            Error = ServerCodes.AccessTokenExpired,
                            Data = JsonDocument.Parse("{}").RootElement
                        };
                    }
                }
                else responce = handler.HandleRequest(request, client);
                
                responce.Number = request.Number;
                return responce;
            }
            else
            {
                _logger.log($"No handler found for request type: {request.Type}", GetType().Name);
                return new Responce
                {
                    Number = request.Number,
                    Type = request.Type,
                    Success = false,
                    Error = ServerCodes.BadRequest,
                    Data = JsonDocument.Parse("{}").RootElement
                };
            }
        }
    }
}
 