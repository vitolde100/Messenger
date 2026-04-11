namespace MessengerServer.RequestHandlers
{
    internal class RequestRegistrar
    {
        public static void RegiterAll(RequestRouter router)
        {
                router.RegisterHandler(new LoginRequestHandler());
                router.RegisterHandler(new RegisterRequestHandler());
                router.RegisterHandler(new SendMessageRequestHandler());
                router.RegisterHandler(new GetMessagesRequestHandler());
                router.RegisterHandler(new GetContactsRequestHandler());
                router.RegisterHandler(new AddContactRequestHandler());
                router.RegisterHandler(new RemoveContactRequestHandler());
                router.RegisterHandler(new CreateGroupRequestHandler());
                router.RegisterHandler(new GetGroupsRequestHandler());
                router.RegisterHandler(new GetGroupMessagesRequestHandler());
                router.RegisterHandler(new SendGroupMessageRequestHandler());
        }
    }
}
