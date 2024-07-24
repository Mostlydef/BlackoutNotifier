using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;


        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
                { InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
                { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;

        }

        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
                return;
            var action = messageText.Split(' ')[0] switch
            {
                "/inline_keyboard" => SendInlineKeyboard(_botClient, message, cancellationToken),
                "/android_id" => SendAndroidId(_botClient, message, cancellationToken),
                "/keyboard" => SendReplyKeyboard(_botClient, message, cancellationToken),
                "/remove" => RemoveKeyboard(_botClient, message, cancellationToken),
                "/inline_mode" => StartInlineQuery(_botClient, message, cancellationToken),
                "/throw" => FailingHandler(_botClient, message, cancellationToken),
                "/start" => Usage(_botClient, message, cancellationToken),
                _ => Usage(_botClient, message, cancellationToken)
            };
            Message sentMessage = await action;
            _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

            static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                await botClient.SendChatActionAsync(
                    chatId: message.Chat.Id,
                    chatAction: ChatAction.Typing,
                    cancellationToken: cancellationToken);

                await Task.Delay(500, cancellationToken);

                InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("1.1", "11"),
                            InlineKeyboardButton.WithCallbackData("1.2", "12"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("2.1", "21"),
                            InlineKeyboardButton.WithCallbackData("2.2", "22"),
                        },
                    });

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> SendAndroidId(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {

                return await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: "/AndroidID",
                   cancellationToken: cancellationToken);
            }

            static async Task<Message> SendReplyKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                        new KeyboardButton[] {"/AndroidID"}
                    })
                { ResizeKeyboard = true };

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "/AndroidID",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> RemoveKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Removing keyboard",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                const string usage = "Usage:\n" +
                                 "/inline_keyboard - send inline keyboard\n" +
                                 "/start    - send custom keyboard\n" +
                                 "/remove      - remove custom keyboard\n" +
                                 "/photo       - send a photo\n" +
                                 "/request     - request location or contact\n" +
                                 "/inline_mode - send keyboard with Inline Query";

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> StartInlineQuery(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                InlineKeyboardMarkup inlineKeyboard = new(
                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Inline Mode"));

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Press Inline Query",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }

            static Task<Message> FailingHandler(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                throw new IndexOutOfRangeException();
            }

        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}",
                cancellationToken: cancellationToken);

            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text:$"Received {callbackQuery.Data}",
                cancellationToken:cancellationToken);
        }

        private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

            InlineQueryResult[] results = {
            new InlineQueryResultArticle(
                id: "1",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent("hello"))
            };

            await _botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                cacheTime: 0,
                isPersonal: true,
                cancellationToken: cancellationToken);
        }

        private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);

            await _botClient.SendTextMessageAsync(
                chatId: chosenInlineResult.From.Id,
                text: $"You chose result with Id: {chosenInlineResult.ResultId}",
                cancellationToken: cancellationToken);
        }

        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }


    }
}
