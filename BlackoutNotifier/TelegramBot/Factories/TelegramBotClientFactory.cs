using Telegram.Bot;

namespace TelegramBot;

public class TelegramBotClientFactory : ITelegramBotClientFactory
{
    public ITelegramBotClientWrapper CreateWrapper(ITelegramBotClient telegramBotClient)
    {
        return new TelegramBotClientWrapper(telegramBotClient);
    }
}