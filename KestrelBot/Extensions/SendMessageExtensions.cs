using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;

namespace KestrelBot.Extensions;

public static class SendMessageExtensions
{
    public static async Task TrySendMessageAsync(this IUser user, string message)
    {
        try
        {
            await user.SendMessageAsync(message);
        }
        catch (HttpException) {}
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    public static async Task<IUserMessage?> TrySendMessageAsync(this IMessageChannel channel, string message)
    {
        try
        {
            return await channel.SendMessageAsync(message);
        }
        catch (HttpException) {}
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}