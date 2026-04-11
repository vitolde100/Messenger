using MessengerShared.Requests;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MessengerServer.Requests
{
    internal abstract class IRequestHandler
    {
        public string Type;
  
        public IRequestHandler()
        {
            Type = GetType().Name;
        }

        public abstract Responce HandleRequest(JsonElement data);

        public bool Validate(object obj)
        {
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
