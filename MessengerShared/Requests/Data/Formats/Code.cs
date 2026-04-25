using MessengerShared.Requests.Enums;
using System.Text.Json.Serialization;

namespace MessengerShared.Requests.Data.Formats
{
    public class Code : IEnvelopePayload
    {
        [JsonIgnore]
        public EnvelopeTypes EnvelopeType => EnvelopeTypes.Code;
            
        ServerCodes code { get; set; }

        public Code(ServerCodes code)
        {
            this.code = code;
        }
    }
}
