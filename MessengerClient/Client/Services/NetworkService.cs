using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using MessengerClient.Client;
using MessengerClient.Client.Protocol;
using System.Text.Json;

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

        public async Task<Responce> Login() 
        {
            var Data = new UserData { Login = State.Login, Password = State.Password };
            var Request = new Request(null, "Login", Data);
            return await _protocol.SendAndReciveAsync(Request);
        }

        public async Task<Responce> Registrate() 
        {
            var Data = new UserData { Login = State.Login, Password = State.Password };
            var Request = new Request(null, "Registration", Data);
            return await _protocol.SendAndReciveAsync(Request);
        }

        public async Task Logout() 
        {
            var Request = new Request(State.Session.accessToken, "Logout", null);
            await _protocol.SendAsync(Request);
        }

        public async Task<Responce> SendMessage(ChatMessageData data) 
        { 
            var Request = new Request(State.Session.accessToken, "SendMessage",data);
            return await _protocol.SendAndReciveAsync(Request);
        } 
    }
}