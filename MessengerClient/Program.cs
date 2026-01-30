using System.Net;
using System.Text;

namespace MessengerClient
{
    internal static class Program
    {
        public static string NickName;
        public static string IP;
        public static int Port;
        public static Client client = new Client();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Form registation = new Registration();
            Form chat = new Form();
            Application.Run(registation);
            Application.Run(chat);
        }
    }
}