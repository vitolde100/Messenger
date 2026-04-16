using MessengerServer.Core;
using MessengerServer.Requests;
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
        private Logger _logger = Logger.instance;

        public RequestRouter(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public void RegisterHandler(IRequestHandler handler)
        {
            _handlers[handler.Type] = handler;
        }

        public Responce ProcessRequest(Request request, ClientContext context)
        {
            if (_handlers.TryGetValue(request.Type, out var handler))
            {
                var responce = new Responce();
                if (!_sessionService.isSessionValid(request.AccessToken) && handler.ShouldBeAutorised)
                {
                    responce = new Responce
                    {
                        Type = request.Type,
                        Success = false,
                        Error = ServerCodes.Unauthorized,
                        Data = JsonDocument.Parse("{}").RootElement
                    };
                }
                else responce = handler.HandleRequest(request.Data, context);

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
 