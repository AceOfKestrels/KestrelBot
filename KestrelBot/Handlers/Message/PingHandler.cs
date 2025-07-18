using Discord;
using Discord.Net;
using Discord.WebSocket;
using KestrelBot.Extensions;

namespace KestrelBot.Handlers.Message;

public class PingHandler: MessageHandler
{
    private const int Threshold = 5;
    private int _pingCount;
    
    protected override async Task Handle(SocketMessage message)
    {
        if (!message.MentionedUsers.Contains(Client.CurrentUser))
            return;

        _pingCount++;

        int randomDelay = Random.Shared.Next(100, 1000);
        await Task.Delay(randomDelay);

        if (_pingCount < Threshold)
            await message.Author.TrySendMessageAsync(message.Author.Mention);
        else
            await PingAll(message.Channel, message.Author);
    }

    private async Task PingAll(ISocketMessageChannel channel, SocketUser author)
    {
        _pingCount = 0;
        var message = await channel.TrySendMessageAsync($"@everyone Threshold reached by {author.Mention}!");
        string? messageUrl = message?.GetJumpUrl();
        
        if(messageUrl is null)
            return;
        
        await channel.TrySendMessageAsync(
            "https://tenor.com/view/yippee-happy-yippee-creature-yippee-meme-yippee-gif-gif-1489386840712152603");
        
        var users = await channel.GetUsersAsync().FlattenAsync();
        foreach (var user in users)
        {
            if(user.IsBot)
                continue;
            
            await user.TrySendMessageAsync($"{user.Mention} {messageUrl}");
        }
    }
}