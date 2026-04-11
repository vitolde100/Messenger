using MessengerServer.Requests;
using MessengerShared.Requests;

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
                return handler.HandleRequest(request.Data);
            }
            else
            {
                _logger.log($"No handler found for request type: {request.Type}", GetType().Name);
                return null;
            }
        }
    }
}
 