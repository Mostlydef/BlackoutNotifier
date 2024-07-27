using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot;

public class TelegramBotClientWrapper : ITelegramBotClientWrapper
{
    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramBotClientWrapper(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public Task<Message> SendTextMessageAsync(long chatId, string text, CancellationToken cancellationToken)
    {
        return _telegramBotClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    public Task<Message> SendTextMessageAsyncWithRemoveMarkup(long chatId, string text, CancellationToken cancellationToken)
    {
        return _telegramBotClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
}