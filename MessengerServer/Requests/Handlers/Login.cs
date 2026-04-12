using MessengerServer.Data;
using MessengerShared.Requests;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal class Login : IRequestHandler
    {
        SessionService _sessionService;
        ClientService _clientService;
        public Login(SessionService seService, ClientService clService) : base()
        {
            _sessionService = seService;
            _clientService = clService;
        }

        public override Responce HandleRequest(JsonElement Data)
        {
            var handshake = JsonSerializer.Deserialize<ClientData>(Data);
            if (!Validate(Data)) return null;

            var User = _clientService.GetClientByLogin(handshake.Login);

            if (User == null || BCrypt.Net.BCrypt.Verify(handshake.Password,User.Password))
                return null;

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
