using System.Collections.Concurrent;
using MessengerServer.Data;

namespace MessengerServer.Core
{
    internal class ChatRegistry
    {
        ConcurrentDictionary<string, Chat> _chats = new ConcurrentDictionary<string, Chat>();
        Logger _logger = Logger.instance;

        public ChatRegistry() { }

        public string AddChat(Chat chat)
        {
            try
            {
                chat.Id = Guid.NewGuid().ToString();
                _chats.TryAdd(chat.Id, chat);

                _logger.log($"Added Chat: {chat.Name}", GetType().Name);
                
                return chat.Id;
            }
            catch (Exception ex) 
            {
                _logger.log($"Error Occurred While Save: {ex.Message}", GetType().Name);
                return null;
            }
        }

        public Chat Get(string chatId)
        {
            Chat chat = new Chat();
            try
            {
                _chats.TryGetValue(chatId,out chat);
            }
            catch
            {
            }
                return chat;
        }
    }
}
