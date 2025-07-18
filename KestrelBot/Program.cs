using Discord;
using Discord.WebSocket;
using KestrelBot.Handlers;
using KestrelBot.Handlers.Message;

namespace KestrelBot;

class Program
{
    private static readonly DiscordSocketClient Client = new();	
    
    public static async Task Main(string[] args)
    {
        Client.Log += Log;
        Client.Ready += UnregisterSlashCommands;
        
        string? botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
        if (string.IsNullOrWhiteSpace(botToken))
            throw new ArgumentException("BOT_TOKEN environment variable must be set");

        try
        {
            await Client.LoginAsync(TokenType.Bot, botToken);
            await Client.StartAsync();
        }
        catch (Exception e)
        {
            throw new AggregateException("Could not log in to bot account", e);
        }
        
        RegisterHandler<PingHandler>();
        
        await Task.Delay(-1);
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }

    private static void RegisterHandler<T>() where T:Handler, new()
    {
        T handler = new();
        handler.Init(Client);
    }

    private static Task UnregisterSlashCommands()
    {
        _ = Task.Run(async () =>
        {
            try
            {
                var guilds = Client.Guilds;
                foreach (var socketGuild in guilds)
                    await socketGuild.DeleteApplicationCommandsAsync(RequestOptions.Default);
                await Client.BulkOverwriteGlobalApplicationCommandsAsync([], RequestOptions.Default);
            }
            catch (Exception)
            {
                // ignored
            }
        });
        return Task.CompletedTask;
    }
}