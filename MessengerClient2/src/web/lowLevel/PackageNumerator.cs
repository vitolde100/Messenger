namespace MessengerClient2.src.web.lowLevel
{
    /// <summary>
    /// Позволяет получить уникальный аутентификатор пакета
    /// </summary>
    static class PackageNumerator
    {
        private static double currentId = 0;

        public static double getPackageId()
        {
            currentId++;
            return currentId; 
        }
    }
}
