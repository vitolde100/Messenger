using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MessengerShared.API
{
    /// <summary>
    /// Класс который содержит информацию о сессии
    /// </summary>
    public class Session
    {
        public string accessToken;
        public string refreshToken;
        public DateTime access_expires;
        public DateTime refresh_expires;

        //SERVER ONLY
        public string userID;

        /// <summary>
        /// Инициализация на стороне сервера
        /// </summary>
        /// <param name="access"></param>
        /// <param name="refresh"></param>
        public Session(string access, string refresh, string ID)
        {
            accessToken = access;
            refreshToken = refresh;
            access_expires = DateTime.UtcNow.AddHours(1);
            refresh_expires = DateTime.UtcNow.AddDays(7);
            userID = ID;
        }

        /// <summary>
        /// Инициализация полученого по сети пакета сессии
        /// </summary>
        /// <param name="package"></param>
        public Session(string package)
        {
            var doc = JsonDocument.Parse(package);
            accessToken = doc.RootElement.GetProperty("o").GetString();
            refreshToken = doc.RootElement.GetProperty("c").GetString();
            access_expires = doc.RootElement.GetProperty("ae").GetDateTime();
            refresh_expires = doc.RootElement.GetProperty("re").GetDateTime();
        }
        /// <summary>
        /// Упаковка для отправки по сети
        /// </summary>
        /// <returns>Пакет для отправки</returns>
        public string ConvertToPackage()
        {
            if (accessToken != null)
            {
                var data = new
                {
                    o = accessToken,
                    c = refreshToken,
                    ae = access_expires,
                    re = refresh_expires
                };
                return JsonSerializer.Serialize(data);
            }
            return null;
        }

        public bool isAccessValid()
        {
            return access_expires.CompareTo(DateTime.UtcNow) > 0;
        }

        public bool isRefreshValid()
        {
            return access_expires.CompareTo(DateTime.UtcNow) > 0;
        }

        public bool isMathches(Session ses)
        {
            return ses.accessToken.Equals(accessToken) && ses.refreshToken.Equals(refreshToken);
        }
    }
}
