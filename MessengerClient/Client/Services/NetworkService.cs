using MessengerClient.Client;
using MessengerClient.Client.Protocol;
using MessengerShared.API;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MessengerClient.Client.Services
{
    public class NetworkService
    {
        private IProtocol _protocol;
        private AuthService _authService;
        public NetworkService(IProtocol protocol, AuthService authService) 
        { 
            _protocol = protocol;
            _authService = authService;
        }

        public async Task<Response> Login(string Login, string Password) 
        {
            var Data = new UserData { Login = Login, Password = Password };
            var Request = new Request(null, "Login", Data);
            var Responce = await _protocol.SendAndReciveAsync(Request);
            Responce.Data = GetData<Session>(Responce);
            return Responce;
        }

        public async Task<Response> Registrate(string Login, string Password) 
        {
            var Data = new UserData { Login = Login, Password = Password };
            var Request = new Request(null, "Registration", Data);
            var Responce = await _protocol.SendAndReciveAsync(Request);
            Responce.Data = GetData<Session>(Responce);
            return Responce;
        }

        public async Task<Response> Logout() 
        {
            var Request = new Request(Program.state.Session.accessToken, "Logout", null);
            return await _protocol.SendAndReciveAsync(Request);
        }

        public async Task<Response> SendMessage(ChatMessageData data) 
        { 
            var Request = new Request(Program.state.Session.accessToken, "SendMessage",data);
            return await _authService.SendWithAuth(Request);
        } 

        public async Task<Response> CreateChat()
        {
            var Request = new Request(Program.state.Session.accessToken, "CreateChat", null);
            var Responce = await _authService.SendWithAuth(Request);
            Responce.Data = GetData<string>(Responce);
            return Responce;
        }

        public async Task<Response> GetContact(string FriendID)
        {
            var Request = new Request(Program.state.Session.accessToken, "GetContact", FriendID);
            var Responce = await _authService.SendWithAuth(Request);
            Responce.Data = GetData<ContactData>(Responce);
            return Responce;
        } 

        private static T? GetData<T>(Response response)
        {
            if (response.Data is not JsonElement element)
                return default;

            if (element.ValueKind == JsonValueKind.Object && !element.EnumerateObject().Any())
                return default;

            if (element.ValueKind == JsonValueKind.Null)
                return default;

            try
            {
                return element.Deserialize<T>();
            }
            catch
            {
                return default;
            }
        }
    }
}