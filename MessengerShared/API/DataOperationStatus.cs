using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessengerShared.API
{
    public class DataOperationStatus
    {
        public bool success { get; private set; }
        public string errorMessage { get; private set; }

        /// <summary>
        /// Создание на стороне сервера
        /// </summary>
        /// <param name="success">Успешная ли операция</param>
        /// <param name="errorMessage">Описание ошибки или пустое поле</param>
        public DataOperationStatus(bool success, string errorMessage)
        {
            this.success = success;
            this.errorMessage = errorMessage;
        }
        /// <summary>
        /// Инициализация полученого по сети статуса
        /// </summary>
        /// <param name="package"></param>
        public DataOperationStatus(string package)
        {
            var doc = JsonDocument.Parse(package);
            success = doc.RootElement.GetProperty("s").GetBoolean();
            errorMessage = doc.RootElement.GetProperty("e").GetString();
        }

        /// <summary>
        /// Упаковка для отправки по сети
        /// </summary>
        /// <returns>Пакет для отправки</returns>
        public string ConvertToPackage()
        {
            var data = new
            {
                s = success,
                e = errorMessage
            };
        return JsonSerializer.Serialize(data);
        }

    }
}
