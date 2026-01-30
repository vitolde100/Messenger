namespace MessengerServer
{
    public class Logger
    {
        private readonly object m_lock = new object();
        private static readonly Logger m_instance = new Logger();
        private Logger()
        {
        }
        public static Logger instance
        {
            get { return m_instance; }
        }

        public void log(string message, string sender)
        {
            lock (m_lock)
            {
                Console.WriteLine("["+ sender +"]:" + message);
            }
        }
    }
}
