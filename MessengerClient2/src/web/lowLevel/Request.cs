namespace MessengerClient2.src.web.lowLevel
{
    /// <summary>
    /// Реквест к серверу
    /// </summary>
    class Request
    {
        string keyword;
        double reqFlag;
        string PORNO;
        string msg;
        int reqCounter = 0;
        private SemaphoreSlim _signal = new SemaphoreSlim(0, 1);
        public async  Task<string> SendRequest(string keyword, string package)
        {
            this.keyword = keyword;
            reqFlag = PackageNumerator.getPackageId();
            /// Запрос к серверу
            /// reqFlag - уникальный номер реквеста
            /// keyword - тип запроса к серверу
            /// package - данные
            /// В качестве разделителя использован символ 卐
            msg = reqFlag.ToString() + "卐" + keyword + "卐" + package;
            ClientConnectionHandler.SendMessage(msg);
            ServerUpdateEventHandler.AddMethodToSubscribers(Subscriber);
            await _signal.WaitAsync();
            return PORNO;
        }

        public void Subscriber()
        {
            if (ServerUpdateEventHandler.currentMessage.Split('卐', 3)[0] == reqFlag.ToString())
            {
                PORNO = ServerUpdateEventHandler.currentMessage.Split('卐', 2)[1];
                _signal.Release();
            }
            else { if (reqCounter < 25) { ServerUpdateEventHandler.AddMethodToSubscribers(Subscriber); reqCounter++; } else { reqCounter = 0; ClientConnectionHandler.SendMessage(msg); } }
        }
    }
}
