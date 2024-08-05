using Telegram.Bot.Requests;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;

namespace TelegramBot;

public interface ITelegramBotClientWrapper
{
    Task<Message> SendTextMessageAsync(long chatId, string text, CancellationToken cancellationToken);
    Task<Message> SendTextMessageAsyncWithRemoveMarkup(long chatId, string text, CancellationToken cancellationToken);
    Task<User> GetMeAsync(CancellationToken cancellationToken = default);
    Task ReceiveAsync(IUpdateHandler updateHandler, ReceiverOptions? receiverOptions = default, CancellationToken cancellationToken = default);
}