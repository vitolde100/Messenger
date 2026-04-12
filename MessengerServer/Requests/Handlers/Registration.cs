using MessengerServer.Data;
using MessengerShared.API;
using MessengerShared.Requests;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Registration : IRequestHandler
    {
        SessionService _sessionService;
        ClientService _clientService;
        public Registration(SessionService seService, ClientService clService) : base()
        { 
            _sessionService = seService;
            _clientService = clService;
        }

        public override Responce HandleRequest(JsonElement Data)
        {
            var handshake = JsonSerializer.Deserialize<ClientData>(Data);
            if (!Validate(Data)) return null;

            var User = _clientService.CreateClient(handshake.Login, handshake.Password);
            var session = _sessionService.CreateSession(User.ID);

            return new Responce
            {
                Type = GetType().Name,
                Success = true,
                Data = session.ConvertToPackage()
            };
        }
    }
}
