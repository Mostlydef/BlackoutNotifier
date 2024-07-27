using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot;

public interface ITelegramBotClientWrapper
{
    Task<Message> SendTextMessageAsync(long chatId, string text, CancellationToken cancellationToken);
    Task<Message> SendTextMessageAsyncWithRemoveMarkup(long chatId, string text, CancellationToken cancellationToken);
}