using MessengerServer.Core;
using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data.Chats;
using MessengerShared.Requests.Enums;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class AddToChat : RequestHandler
    {
        ChatRegistry _chatRegistry;
        ClientRegistry _clientRegistry;
        MessagingService _messagingService;
        public AddToChat(ChatRegistry chatRegistry, ClientRegistry clientRegistry, MessagingService messagingService) 
        { 
            _chatRegistry = chatRegistry;  
            _clientRegistry = clientRegistry;
            _messagingService = messagingService;
            ShouldBeAutorised = true;
        }

        public override Response Handle(Request request, ClientHandler client)
        {
            var Data = JsonSerializer.Deserialize<AddToChatData>(JsonSerializer.Serialize(request.Data));
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            var user = _clientRegistry.Get(Data.UserId);

            if (user == null) return BuildResponce(ServerCodes.NoTargetUser);

            var chat = _chatRegistry.Get(Data.UserId);

            if (chat == null) return BuildResponce(ServerCodes.NoTargetChat);
            
            chat.AddUser(Data.UserId);

            _messagingService.SendMessage(new ChatEventData { ChatId = chat.Id, EventCode = ChatEventCodes.Added }, Data.UserId);

            return BuildResponce();
        }
    }
}