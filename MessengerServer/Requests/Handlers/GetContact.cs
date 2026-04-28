using MessengerServer.Data;
using MessengerServer.Services;
using MessengerShared.Requests;
using MessengerShared.Requests.Data.Formats;
using MessengerShared.Requests.Enums;
using System.Text.Json;
namespace MessengerServer.Requests.Handlers
{
    internal class GetContact : RequestHandler
    {
        private ClientService _clientService;
        public GetContact(ClientService clientService)
        {
            _clientService = clientService;
            ShouldBeAutorised = true;
        }

        public override Response Handle(Request request, ClientHandler client)
        {
            string Login = JsonSerializer.Deserialize<string>(JsonSerializer.Serialize(request.Data));
            if (Login == null) return BuildResponce(ServerCodes.BadRequest);

            var contact = _clientService.GetClientByLogin(Login);

            return BuildResponce(new ContactData { name = contact.Login, userId = contact.ID});
        }
    }
}
