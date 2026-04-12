using MessengerServer.Requests;
using MessengerShared.Requests;
using System.Text.Json;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRouter
    {
        private Dictionary<string, IRequestHandler> _handlers = new Dictionary<string, IRequestHandler>();

        Logger _logger = Logger.instance;

        public void RegisterHandler(IRequestHandler handler)
        {
            _handlers[handler.Type] = handler;
        }

        public Responce ProcessRequest(Request request)
        {
            if (_handlers.TryGetValue(request.Type, out var handler))
            {
                var responce = handler.HandleRequest(request.Data);
                if (responce == null) responce = new Responce{ Success = false, Data = JsonDocument.Parse("{}").RootElement };
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
                    Data = JsonDocument.Parse("{}").RootElement
                };
            }
        }
    }
}
 