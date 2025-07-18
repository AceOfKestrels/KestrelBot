using Discord.WebSocket;

namespace KestrelBot.Handlers;

public abstract class Handler
{
    protected DiscordSocketClient Client { get; private set; } = new();
    
    public void Init(DiscordSocketClient client)
    {
        Client = client;
        InitInternal(client);
    }

    protected abstract void InitInternal(DiscordSocketClient client);
}