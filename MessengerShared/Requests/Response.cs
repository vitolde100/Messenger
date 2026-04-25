using MessengerShared.Requests.Data;
using MessengerShared.Requests.Enums;
using System.Text.Json.Serialization;

namespace MessengerShared.Requests 
{ 
    public class Response : IEnvelopePayload
    {
        [JsonIgnore]
        public EnvelopeTypes EnvelopeType => EnvelopeTypes.Response;

        public int Number { get; set; }
        public ServerCodes Error { get; set; } = ServerCodes.NoErrors;
        public string RequestType { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }

    }
}
