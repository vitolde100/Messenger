using MessengerClient2.src.web.lowLevel;

namespace MessengerClient2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Client());
            Test();
        }

        static async void Test()
        {
            Request r = new Request();
            Task<string> t = r.SendRequest("huy", "pipo");
            ServerUpdateEventHandler.Invoke("333卐heh");
            ServerUpdateEventHandler.Invoke("1卐hah");
            string msg = t.Result;
            MessageBox.Show(msg);
        }
    }
}