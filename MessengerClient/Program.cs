using MessengerShared;
using System.Net;

namespace MessengerClient
{
    internal static class Program
    {
        public static string NickName = "Test";
        public static string IP = "192.168.1.2";
        public static int Port = Protocol.DefaultPort;
        public static bool isGuest = true;
        public static bool isConnected = false;
        public static Client client = new Client();

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Form Welcome = new WelcomeForm();
            if (isGuest) 
                Application.Run(Welcome);
            
            if(!isConnected)
                Application.Run(Welcome);

            Form Chat = new ChatForm();
            Application.Run(Chat);
            //Test_Form Test = new Test_Form();
            //Application.Run(Test);
        }
    }
}