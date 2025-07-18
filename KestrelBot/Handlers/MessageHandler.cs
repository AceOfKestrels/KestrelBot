using System.Runtime.InteropServices.ComTypes;
using Discord.WebSocket;

namespace KestrelBot.Handlers;

public abstract class MessageHandler : Handler
{
    protected override void InitInternal(DiscordSocketClient client)
    {
        client.MessageReceived += msg => _ = HandleInternal(msg);
    }

    private async Task HandleInternal(SocketMessage message)
    {
        try
        {
            await Handle(message);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }
    }

    protected abstract Task Handle(SocketMessage message);
}