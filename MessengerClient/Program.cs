using MessengerShared;
using System.Net;

namespace MessengerClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Form Welcome = new WelcomeForm();
            Form Chat = new ChatForm();

            Application.Run(Welcome);
            Application.Run(Chat);
            //Test_Form Test = new Test_Form();
            //Application.Run(Test);
        }
    }
}