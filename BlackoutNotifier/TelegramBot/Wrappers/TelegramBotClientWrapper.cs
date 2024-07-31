using System.Data;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Services;

namespace TelegramBot;

[ExcludeFromCodeCoverage]
public class TelegramBotClientWrapper : ITelegramBotClientWrapper
{
    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramBotClientWrapper(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task<User> GetMeAsync(CancellationToken cancellationToken = default)
    {
        if(_telegramBotClient != null)
        {
            return await _telegramBotClient
            .MakeRequestAsync(request: new GetMeRequest(), cancellationToken)
            .ConfigureAwait(false);
        }
        throw new NullReferenceException();
    }

    public async Task ReceiveAsync(IUpdateHandler updateHandler, ReceiverOptions? receiverOptions = null, CancellationToken cancellationToken = default)
    {
        await _telegramBotClient.ReceiveAsync(updateHandler: updateHandler, receiverOptions: receiverOptions, cancellationToken
                : cancellationToken);
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