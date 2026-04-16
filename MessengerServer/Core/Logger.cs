namespace MessengerServer.Core
{
    public class Logger
    {
        private readonly object _lock = new object();
        private static readonly Logger _instance = new Logger();
        private Logger()
        {
        }
        public static Logger instance
        {
            get { return _instance; }
        }

        public void log(string message, string sender)
        {
            lock (_lock)
            {
                Console.WriteLine("["+ sender +"]:" + message);
            }
        }
    }
}
