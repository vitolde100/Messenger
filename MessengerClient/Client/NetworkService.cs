using MessengerShared.Requests;
using MessengerShared.Requests.Data;
using System.Text.Json;

namespace MessengerClient.Client
{
    internal class NetworkService
    {
        private Protocol.IProtocol _protocol;
        public NetworkService(Protocol.IProtocol protocol) 
        { 
            _protocol = protocol;
        }

        public Responce Login(string login, string password) 
        {
            var Data = new UserData { Login = login, Password = password };
            var Request = new Request(State.session.accessToken, "Login", BuildJsonElement(Data));
            return _protocol.SendAndReciveAsync(Request).Result;
        }

        public Responce Registrate(string login, string password) 
        {
            var Data = new UserData { Login = login, Password = password };
            var Request = new Request(State.session.accessToken, "Registration", BuildJsonElement(Data));
            return _protocol.SendAndReciveAsync(Request).Result;
        }

        public Responce SendMessage(string text, string targetID) 
        { 
            var Data = new ChatMessageData { SendTime = DateTime.Now.TimeOfDay, TargetID = targetID, Text = text };
            var Request = new Request(State.session.accessToken, "SendMessage", BuildJsonElement(Data));
            return _protocol.SendAndReciveAsync(Request).Result;
        }

        public void Logout() 
        {
            var Request = new Request(State.session.accessToken, "Logout", BuildJsonElement(null));
            _protocol.SendAsync(Request);
        }

        private JsonElement BuildJsonElement(object data)  
        {
            var json = JsonSerializer.Serialize(data);
            return JsonSerializer.Deserialize<JsonElement>(json);
        }
    }
}