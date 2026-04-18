using MessengerShared.Requests;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MessengerServer.Requests.Handlers
{
    internal abstract class IRequestHandler
    {
        public string Type;
        public bool ShouldBeAutorised = false;
        public IRequestHandler()
        {
            Type = GetType().Name;
        }

        public abstract Responce HandleRequest(Request request, ClientContext context);

        protected Responce BuildResponce(ServerCodes code = ServerCodes.NoErrors)
        {
            var responce = new Responce
            {
                Type = GetType().Name,
                Error = code
            };
            responce.Success = code == ServerCodes.NoErrors;
            return responce;
        }

        protected Responce BuildResponce(JsonElement Data)
        {
            return new Responce
            {
                Type = GetType().Name,
                Success = true,
                Data = Data
            };
        }

        public bool Validate(object? obj)
        {
            if (obj == null) return false;
            var props = obj.GetType().GetProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);

                if (Attribute.IsDefined(prop, typeof(RequiredAttribute)))
                {
                    if (value == null || (value is string s && string.IsNullOrEmpty(s)))
                        return false;
                }
            }
            return true;
        }
    }
}
