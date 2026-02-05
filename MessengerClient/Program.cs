using System.Net;
using System.Text;

namespace MessengerClient
{
    internal static class Program
    {
        public static string NickName;
        public static string IP;
        public static int Port;
        public static bool isConnected = false;
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
            /*if (!isConnected)
            {
                Form Welcome = new Welcome_window();
                Application.Run(Welcome);
            }
            if (isConnected)
            {
                Form Chat = new ChatForm();
                Application.Run(Chat);
            }*/
            Form Chat = new ChatForm();
            Application.Run(Chat);
        }
    }
}