using System.Text.Json;

namespace MessengerShared.Requests 
{ 
    public class Responce
    {
        public string Type;
        public bool Success;
        public JsonElement Data; 
    }
}
