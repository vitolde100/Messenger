using MessengerClient2.src.clientDB;
using MessengerClient2.src.web.lowLevel;
using MessengerClient2.windows;

namespace MessengerClient2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            if (!ClientDBHandler.IsDBExists())
            {
                ClientDBHandler.data = new ClientDBHandler.HandledData();
                //Ask to log in or register
                Application.Run(new Hello());
            }
            else
            {
                ClientDBHandler.Load();
            }
            
        }
    }
}