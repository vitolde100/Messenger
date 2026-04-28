using MessengerServer.Core;
using MessengerShared.Requests.Data;
using MessengerShared.Requests.Enums;
using MessengerShared.Requests;

namespace MessengerServer.Services
{
    internal class MessagingService
    {
        ClientRegistry registry;
        ClientService _clientService;
        SessionService _sessionService;

        public MessagingService(ClientRegistry registry, ClientService clientService, SessionService sessionService) 
        { 
            this.registry = registry;
            _clientService = clientService;
            _sessionService = sessionService;
        }

        public async Task<bool> SendMessage(IEnvelopePayload msg, string targetUserId)
        {
            var client = registry.Get(targetUserId);

            if (client == null)
                return false;

            await client.Send(msg);
            return true;
        }
    }
}