using System.Text.Json;

namespace MessengerShared.Requests 
{ 
    public class Response
    {
        public int Number { get; set; }
        public ServerCodes Error { get; set; } = ServerCodes.NoErrors;
        public string Type { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
    }
}
