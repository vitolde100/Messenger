using MessengerShared;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MessengerClient2.src.web.lowLevel
{
    public static class ClientConnectionHandler
    {
        private static TcpClient tcpClient = new TcpClient();
        private static Stream stream;

        public static bool connectionStatus = false;

        #region НИЗКОУРОВНЕВОЕ СОЕДИНЕНИЕ / ОТСОЕДИНЕНИЕ / ЧТЕНИЕ / ОТПРАВКА

        /// <summary>
        /// УЯЗВИМОСТЬ WARNING УЯЗВИМОСТЬ WARNING УЯЗВИМОСТЬ WARNING
        /// Не понятно зачем нужен, всегда возращает true
        /// </summary>
        static bool ValidateServerCertificate(
            object sender,
            X509Certificate cert,
            X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// Установление соединения с сервером
        /// </summary>
        /// <param name="errorHandler">Метод который будет вызван в случае ошибки</param>
        /// <param name="successHandler">Метод который будет вызван в случае успешного соединения</param>
        public static async void ConnectToServer(string host, int port, Action errorHandler, Action successHandler)
        {
            try
            {
                await tcpClient.ConnectAsync(host, port);

                var ssl = new SslStream(
                tcpClient.GetStream(),
                false,
                ValidateServerCertificate);

                await ssl.AuthenticateAsClientAsync(host);

                stream = ssl;
                connectionStatus = stream.CanRead && stream.CanWrite;
                if (!connectionStatus)
                {
                    //Connection failure
                    throw new Exception("Connection failure");
                }
                else
                {
                    //Connection success
                    successHandler();
                    await ReadAsync();
                }
            }
            catch (Exception ex)
            {
                errorHandler();
#if DEBUG
                throw ex;
#endif
            }
        }

        /// <summary>
        /// Разрыв соединения с сервером
        /// </summary>
        public static void DisconnectFromServer()
        {
            connectionStatus = false;
            try
            {
                stream.Dispose();
                tcpClient.Dispose();
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }

        /// <summary>
        /// Цикл проверки на наличие новых сообщений от сервера
        /// </summary>
        public static async Task ReadAsync()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (connectionStatus && stream != null)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        DisconnectFromServer();
                        break;
                    }

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    ServerUpdateEventHandler.Invoke(msg);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }

        /// <summary>
        /// Отправка простейшего сообщения на сервер
        /// </summary>
        public static Task SendMessage(string message)
        {
            try
            {
                if (connectionStatus)
                {
                    byte[] buffer = UTF8Encoding.UTF8.GetBytes(message);
                    stream.Write(buffer, 0, buffer.Length);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
                return Task.CompletedTask;
            }
        }



        #endregion

    }
}
