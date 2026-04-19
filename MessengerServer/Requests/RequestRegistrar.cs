// Почему так костыльно?
// По качану, я тоже не хочу их регать вручную,
// Но так проще, так что просто закрой файл и не думай об этом!
using MessengerServer.Core;
using MessengerServer.Data;
using MessengerServer.Requests.Handlers;
using MessengerServer.Services;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRegistrar
    {
        public static void RegiterAll(RequestRouter router, SessionService sessionServise, ClientService clientService, MessagingService messagingService,ClientRegistry registry)
        {
            router.RegisterHandler(new Login(sessionServise, clientService, registry));
            router.RegisterHandler(new Registration(sessionServise, clientService, registry));
            router.RegisterHandler(new Logout(sessionServise,registry));
            router.RegisterHandler(new Refresh(sessionServise));
            router.RegisterHandler(new SendMessage(messagingService));
        }
    }
}
