using System.Diagnostics.CodeAnalysis;
using Telegram.Bot;

namespace TelegramBot;

[ExcludeFromCodeCoverage]
public class TelegramBotClientFactory : ITelegramBotClientFactory
{
    public ITelegramBotClientWrapper CreateWrapper(ITelegramBotClient telegramBotClient)
    {
        return new TelegramBotClientWrapper(telegramBotClient);
    }
}