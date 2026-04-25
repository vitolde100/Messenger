using MessengerShared.Requests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessengerShared.Requests.Data.Chats
{

    public class ChatEventData : IEnvelopePayload
    {
        [JsonIgnore]
        public EnvelopeTypes EnvelopeType => EnvelopeTypes.ChatEvent;

        public ChatEventCodes EventCode { get; set; }
        public string ChatId { get; set; }
    }
}
