using MessengerClient2.src.web.lowLevel;
using MessengerShared.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MessengerClient2.src.web.highLevel
{
    /// <summary>
    /// Основной класс для отправки данных на сервер
    /// </summary>
    internal static class ClientWebInterface
    {
        /// <summary>
        /// Нужен для получения ключей сессий
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public static async Task<Session> Auth(string login, string password)
        {
            var Data = new { l = login, p = password };
            Request request = new Request();
            return new Session(request.SendRequest("auth", JsonSerializer.Serialize(Data)).Result);
        }
    }
}
