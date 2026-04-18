using System.Text.Json;

namespace MessengerShared.Requests
{
    public class Envelope
    {
        public string Type { get; set; } // "response", "chat", "server"
        public JsonElement Payload { get; set; }
    }
}
