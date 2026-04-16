using MessengerServer.Services;
using MessengerShared.Requests.Data;
using MessengerShared.Requests;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class SendMessage : IRequestHandler
    {
        MessagingService _messagingService;
        public SendMessage(MessagingService messagingService)
        {
            _messagingService = messagingService;

            ShouldBeAutorised = true;
        }

        public override Responce HandleRequest(JsonElement json, ClientContext context)
        {
            var Data = JsonSerializer.Deserialize<ChatMessageData>(json);
            if (Data == null) return BuildResponce(ServerCodes.BadRequest);

            try
            {
                _messagingService.SendMessage(Data);
                return BuildResponce();
            }
            catch
            {
                return BuildResponce(ServerCodes.NoTargetUser);
            }
        }
    }
}