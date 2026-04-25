using MessengerServer.Core;
using MessengerServer.Services;
using MessengerShared.Requests.Data;
using MessengerShared.Requests;
using System.Text.Json;
using MessengerShared.Requests.Enums;

namespace MessengerServer.Requests.Handlers
{
    internal class SendMessage : RequestHandler
    {
        MessagingService _messagingService;
        public SendMessage(MessagingService messagingService)
        {
            _messagingService = messagingService;

            ShouldBeAutorised = true;
        }

        public override Response Handle(Request request, ClientHandler client)
        {
            var Data = JsonSerializer.Deserialize<ChatMessageData>(JsonSerializer.Serialize(request.Data));
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

            if(_messagingService.SendMessage(Data))
                return BuildResponce();
          
            return BuildResponce(ServerCodes.NoTargetUser);
        }
    }
}