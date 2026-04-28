using MessengerServer.Core;
using MessengerServer.Requests.Handlers;
using MessengerServer.Services;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRegistrar
    {
        public static void RegiterAll(RequestRouter router, 
            SessionService sessionServise, ClientService clientService, MessagingService messagingService,
            ClientRegistry clientRegistry, ChatRegistry chatRegistry)
        {
            router.RegisterHandler(new Login(sessionServise, clientService, clientRegistry));
            router.RegisterHandler(new Registration(sessionServise, clientService, clientRegistry));
            router.RegisterHandler(new Logout(sessionServise,clientRegistry));
            router.RegisterHandler(new Refresh(sessionServise));
            router.RegisterHandler(new SendMessage(messagingService));
            router.RegisterHandler(new GetContact(clientService));
            router.RegisterHandler(new CreateChat(chatRegistry));
            router.RegisterHandler(new AddToChat(chatRegistry, clientRegistry, messagingService));
        }
    }
}
