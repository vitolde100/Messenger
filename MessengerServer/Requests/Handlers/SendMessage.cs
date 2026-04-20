using MessengerServer.Core;
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

        public override Responce HandleRequest(Request request, ClientHandler client)
        {
            var Data = JsonSerializer.Deserialize<ChatMessageData>(JsonSerializer.Serialize(request.Data));
            if (!Validate(Data)) return BuildResponce(ServerCodes.BadRequest);

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