using MessengerServer.Core;
using MessengerServer.Data;
using MessengerShared.Requests;
using System.Text.Json;
using MessengerShared.Requests.Data;
using MessengerShared.Requests.Enums;

namespace MessengerServer.Requests.Handlers
{
    internal class CreateChat : RequestHandler
    {
        private ChatRegistry _chatRegistry;
        public CreateChat(ChatRegistry chatRegistry)
        {
            _chatRegistry = chatRegistry;
            ShouldBeAutorised = true;
        }

        public override Response Handle(Request request, ClientHandler client)
        {
            var Data = JsonSerializer.Deserialize<CreateChatData>(JsonSerializer.Serialize(request.Data));
            if (!Validate(Data)) BuildResponce(ServerCodes.BadRequest);

            if(string.IsNullOrEmpty(Data.Name)) return BuildResponce(ServerCodes.BadRequest);

            var Chat = new Chat(Data.isPersonal, Data.Name);
            var Id = _chatRegistry.AddChat(Chat);

            if (Id == null) BuildResponce(ServerCodes.BadRequest);
            Data.ChatID = Id;
            return BuildResponce(Data);
        }
    }
}
