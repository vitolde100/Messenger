using MessengerShared.Requests.Enums;

namespace MessengerShared.Requests.Data
{
    public interface IEnvelopePayload
    {
        EnvelopeTypes EnvelopeType { get; }
    }
}
