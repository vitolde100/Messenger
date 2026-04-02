namespace MessengerClient2.src.web.lowLevel
{
    /// <summary>
    /// Позволяет подписаться на изменение потока сообщений
    /// </summary>
    static class ServerUpdateEventHandler
    {
        static private List<Action> subscribers = new List<Action>();
        static private List<Action> permanentSubscribers = new List<Action>();

        public static string currentMessage = "";

        /// <summary>
        /// Добавить подписку на 1 изменение
        /// </summary>
        public static void AddMethodToSubscribers(Action meth)
        {
            subscribers.Add(meth);
        }
        /// <summary>
        /// Добавить подписку на [бесконечное количество] изменение
        /// </summary>
        public static void AddMethodToPermanentSubscribers(Action meth)
        {
            permanentSubscribers.Add(meth);
        }

        /// <summary>
        /// ВЫЗЫВАТЬ ТОЛЬКО ИЗ ПОТОКА ЧТЕНИЯ
        /// </summary>
        public static void Invoke(string message)
        {
            currentMessage = message;
            var s = subscribers.ToArray();
            subscribers.Clear();
            foreach (Action action in s)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw ex;
#endif
                }
            }
            foreach (Action action in permanentSubscribers)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw ex;
#endif
                }
            }
        }
    }
}
