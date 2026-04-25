using MessengerServer.Core;
using MessengerShared.Requests.

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

        public bool SendMessage(IEnvelopePayload message, string TargetID)
        {
            var Clients = _clientRegistry.Get(TargetID);
            if (Clients != null)
            {
                foreach (var Client in Clients) Client.Send(message);
                return true;
            }
            return false;
        }

        public bool SendMessageToChat(IEnvelopePayload message, string ChatID)
        {

        }
    }
}