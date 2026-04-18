using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MessengerShared.Requests
{
    public class Request
    {
        public int Number; //. . .
        public string AccessToken;
        public string Type;
        public JsonElement? Data;

        public Request(string accessToken, string type, JsonElement? data)
        {
            AccessToken = accessToken;
            Type = type;
            Data = data;
        }
    }
}
