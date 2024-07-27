using Telegram.Bot;

namespace TelegramBot;

public interface ITelegramBotClientFactory
{
     ITelegramBotClientWrapper CreateWrapper(ITelegramBotClient telegramBotClient);
}