using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MessengerShared.API
{
    /// <summary>
    /// Класс который содержит информацию о сессии
    /// </summary>
    public class Session
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public DateTime access_expires {  get; set; }
        public DateTime refresh_expires { get; set; }
        public string userID { get; set; }

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
        /// Упаковка для отправки по сети
        /// </summary>
        /// <returns>Пакет для отправки</returns>
        public JsonElement ConvertToElement()
        {
            var data = new
            {
                accessToken,
                refreshToken,
                access_expires,
                refresh_expires
            };
            var json = JsonSerializer.Serialize(data);
            return JsonSerializer.Deserialize<JsonElement>(json);
        }

        public bool IsAccessExpired() 
        {
            return access_expires.CompareTo(DateTime.UtcNow) > 0;
        }

        public bool IsRefreshExpired()
        {
            return refresh_expires.CompareTo(DateTime.UtcNow) > 0;
        }
    }
}
