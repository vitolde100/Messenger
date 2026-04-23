using MessengerServer.Core;
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

        public abstract Response HandleRequest(Request request, ClientHandler handler);

        protected Response BuildResponce(ServerCodes code = ServerCodes.NoErrors)
        {
            var responce = new Response
            {
                Type = GetType().Name,
                Error = code,
                Data = null
            };
            responce.Success = code == ServerCodes.NoErrors;
            return responce;
        }

        protected Response BuildResponce(object Data)
        {
            return new Response
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
