using System.Text.Json;

namespace MessengerShared.Requests
{
    public class Envelope
    {
        public string Type { get; set; } // "response", "chat", "server"
        public object Payload { get; set; }
    }
}
