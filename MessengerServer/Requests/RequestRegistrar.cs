// Почему так костыльно?
// По качану, я тоже не хочу их регать вручную,
// Но так проще, так что просто закрой файл и не думай об этом!
using MessengerServer.Requests.Handlers;

namespace MessengerServer.RequestHandlers
{
    internal class RequestRegistrar
    {
        public static void RegiterAll(RequestRouter router, IStorage storage)
        {
            router.RegisterHandler(new Registration(storage));
        }
    }
}
