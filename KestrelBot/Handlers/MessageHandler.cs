using Discord.WebSocket;

namespace KestrelBot.Handlers;

public abstract class MessageHandler : Handler
{
    protected override void InitInternal(DiscordSocketClient client)
    {
        client.MessageReceived += HandleInternal;
    }

    private Task HandleInternal(SocketMessage message)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await Handle(message);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        });
        return Task.CompletedTask;
    }

    protected abstract Task Handle(SocketMessage message);
}