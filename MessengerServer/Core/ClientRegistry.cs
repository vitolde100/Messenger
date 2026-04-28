using MessengerServer.Core;
using System.Collections.Concurrent;

internal class ClientRegistry
{
    private readonly ConcurrentDictionary<string, ClientHandler> _clients = new();
    private Logger _logger = Logger.instance;

    public void Add(string userId, ClientHandler handler)
    {
        // если уже есть старое соединение — убиваем его
        if (_clients.TryGetValue(userId, out var oldHandler))
        {
            oldHandler.Deauthenticate();
            oldHandler.ForceDisconnect(); // добавим ниже
        }

        _clients[userId] = handler;

        _logger.log($"User registered: {userId}", nameof(ClientRegistry));
    }

    public ClientHandler? Get(string userId)
    {
        _clients.TryGetValue(userId, out var handler);
        return handler;
    }

    public void Remove(string userId)
    {
        _clients.TryRemove(userId, out _);
    }

}