using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MessengerShared.Requests
{
    public class Request
    {
        public int Number { get; set; }
        public string AccessToken { get; set; }
        public string Type { get; set; }
        public object? Data { get; set; }

        public Request(string accessToken, string type, object? data)
        {
            AccessToken = accessToken;
            Type = type;
            Data = data;
        }
    }
}
