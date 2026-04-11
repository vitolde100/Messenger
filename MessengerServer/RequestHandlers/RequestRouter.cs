namespace MessengerServer.RequestHandlers
{
    internal class RequestRouter
    {
        private Dictionary<string, IRequestHandler> _handlers = new Dictionary<string, IRequestHandler>();

        public void RegisterHandler(IRequestHandler handler)
        {
            _handlers[handler.RequestType] = handler;
        }
    }
}
 