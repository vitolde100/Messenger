using MessengerServer.Core;
using MessengerShared.Requests.Data;

namespace MessengerServer.Services
{
    internal class MessagingService
    {
        ClientRegistry _clientRegistry;
        ClientService _clientService;
        SessionService _sessionService;

        public MessagingService(ClientRegistry registry, ClientService clientService, SessionService sessionService) 
        { 
            _clientRegistry = registry;
            _clientService = clientService;
            _sessionService = sessionService;
        }

        public bool SendMessage(ChatMessageData message)
        {
            var Clients = _clientRegistry.GetClient(message.TargetID);
            if (Clients != null)
            {
                foreach (var Client in Clients) Client.Send(message);
                return true;
            }
            return false;
        }
    }
}