using System.Text.Json;

namespace MessengerShared.Requests 
{ 
    public class Responce
    {
        public int Number { get; set; }
        public ServerCodes Error = ServerCodes.NoErrors;
        public string Type { get; set; }
        public bool Success { get; set; }
        public JsonElement Data { get; set; }
        
    }
}
