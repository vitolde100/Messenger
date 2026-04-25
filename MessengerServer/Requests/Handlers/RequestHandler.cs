using MessengerShared.Requests;
using MessengerShared.Requests.Enums;
using System.ComponentModel.DataAnnotations;

namespace MessengerServer.Requests.Handlers
{
    internal abstract class RequestHandler
    {
        public string Type;
        public bool ShouldBeAutorised = false;
        public RequestHandler()
        {
            Type = GetType().Name;
        }

        public abstract Response Handle(Request request, ClientHandler handler);

        protected Response BuildResponce(ServerCodes code = ServerCodes.NoErrors)
        {
            var responce = new Response
            {
                RequestType = GetType().Name,
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
                RequestType = GetType().Name,
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
