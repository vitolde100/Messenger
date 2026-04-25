using MessengerClient.Client.Protocol;
using MessengerShared.API;
using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using MessengerShared.Requests.Data.Formats;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MessengerClient.Client.Services
{
    public class NetworkService
    {
        private IProtocol _protocol = Program.AppContext.Protocol;
        private AuthService _authService = Program.AppContext.AuthService;
        public NetworkService() { }

        public async Task<Response> Login(string Login, string Password) 
        {
            var Data = new UserData { Login = Login, Password = Password };
            var Request = new Request(null, "Login", Data);
            var Responce = await _protocol.SendAndReciveAsync(Request);
            Responce.Data = GetData<Session>(Responce);
            if (Responce.Success) _authService._authenticated = true;
            return Responce;
        }

        public async Task<Response> Registrate(string Login, string Password) 
        {
            var Data = new UserData { Login = Login, Password = Password };
            var Request = new Request(null, "Registration", Data);
            var Responce = await _protocol.SendAndReciveAsync(Request);
            Responce.Data = GetData<Session>(Responce);
            if (Responce.Success) _authService._authenticated = true;
            return Responce;
        }

        public async Task<Response> Logout() 
        {
            var Request = new Request(Program.state.Session.accessToken, "Logout", null);
            _authService._authenticated = false;
            return await _protocol.SendAndReciveAsync(Request);
        }

        public async Task<Response> SendMessage(ChatMessageData data) 
        { 
            var Request = new Request(Program.state.Session.accessToken, "SendMessage",data);
            return await _authService.SendWithAuth(Request);
        } 

        public async Task<Response> CreateChat(bool isPersonal, string Name)
        {
            var Request = new Request(Program.state.Session.accessToken, "CreateChat", new CreateChatData(isPersonal, Name));
            var Responce = await _authService.SendWithAuth(Request);
            Responce.Data = GetData<CreateChatData>(Responce);
            return Responce;
        }
        
        public async Task<Response> AddToChat(string UserId, string ChatId)
        {
            var Request = new Request(Program.state.Session.accessToken, "AddToChat", new AddToChatData { UserId = UserId, GroupId = ChatId });
            var Responce = await _authService.SendWithAuth(Request);
            Responce.Data = GetData<CreateChatData>(Responce);
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