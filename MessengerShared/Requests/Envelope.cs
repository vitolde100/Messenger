using MessengerShared.Requests.Data;
using MessengerShared.Requests.Enums;

namespace MessengerShared.Requests
{
    public partial class Envelope
    {
        public EnvelopeTypes Type { get; set; }
        public object Payload { get; set; }

        public Envelope() { }

        public Envelope(IEnvelopePayload payload)
        {
            Type = payload.EnvelopeType;
            Payload = payload;
        }
    }
}
