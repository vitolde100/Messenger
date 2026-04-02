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
    internal class Session
    {
        public string openedKey;
        public string closedKey;
        private DateTime expires;

        //SERVER ONLY
        public string userLogin;

        /// <summary>
        /// Инициализация на стороне сервера
        /// </summary>
        /// <param name="openedKey"></param>
        /// <param name="closedKey"></param>
        public Session(string openedKey, string closedKey, string userLogin)
        {
            this.openedKey = openedKey;
            this.closedKey = closedKey;
            expires = DateTime.UtcNow;
            expires.AddHours(24);
            this.userLogin = userLogin;
        }

        /// <summary>
        /// Инициализация полученого по сети пакета сессии
        /// </summary>
        /// <param name="package"></param>
        public Session(string package)
        {
            var doc = JsonDocument.Parse(package);
            openedKey = doc.RootElement.GetProperty("o").GetString();
            closedKey = doc.RootElement.GetProperty("c").GetString();
            expires = doc.RootElement.GetProperty("e").GetDateTime();
        }
        /// <summary>
        /// Упаковка для отправки по сети
        /// </summary>
        /// <returns>Пакет для отправки</returns>
        public string ConvertToPackage()
        {
            if (openedKey != null)
            {
                var data = new
                {
                    o = openedKey,
                    c = closedKey,
                    e = expires
                };
                return JsonSerializer.Serialize(data);
            }
            return null;
        }

        public bool isValid()
        {
            return expires.CompareTo(DateTime.UtcNow) > 0;
        }

        public bool isMathches(Session ses)
        {
            return ses.openedKey.Equals(openedKey) && ses.closedKey.Equals(closedKey);
        }
    }
}
