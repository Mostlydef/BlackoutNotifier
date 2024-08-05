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

using TelegramBot.Networking.HttpDataSendler;
using TelegramBot.Networking.Interfaces;
using TelegramBot.Wrappers;

namespace TelegramBot.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ILoggerWrapper<UpdateHandler> _loggerWrapper;
        private readonly IHttpSendler _httpSendler;
        private readonly ITelegramBotClientFactory _telegramBotClientFactory;


        public UpdateHandler(ITelegramBotClientFactory telegramBotClientFactory, ILoggerWrapper<UpdateHandler> loggerWrapper, IHttpSendler httpSendler)
        {
            _loggerWrapper = loggerWrapper;
            _telegramBotClientFactory = telegramBotClientFactory;
            _httpSendler = httpSendler;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var wrapper = _telegramBotClientFactory.CreateWrapper(botClient);
            
            if(update.Type == UpdateType.Message && update.Message != null)
            {
                await BotOnMessageReceived(wrapper, update.Message, cancellationToken);
            }
            await UnknownUpdateHandlerAsync(update, cancellationToken);
        }

        private async Task BotOnMessageReceived(ITelegramBotClientWrapper botClient, Message message, CancellationToken cancellationToken)
        {
            string messageText;
            _loggerWrapper.LogInformation("Receive message type: {MessageType}", message.Type);
            if (String.IsNullOrWhiteSpace(message.Text))
                return;
            else
                messageText = message.Text;
            Message sentMessage;
            switch (messageText)
            {
                case "/android_id":
                    sentMessage = await OnStart(botClient, message, cancellationToken);
                    break;
                case "/throw":
                    sentMessage = await FailingHandler(botClient, message, cancellationToken);
                    break;
                case "/start":
                    sentMessage = await OnStart(botClient, message, cancellationToken);
                    break;
                default:
                    sentMessage = await Usage(botClient, message, _httpSendler, cancellationToken);
                    break;
            }

            _loggerWrapper.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
        }

        static async Task<Message> OnStart(ITelegramBotClientWrapper botClient, Message message, CancellationToken cancellationToken)
        {
            return await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: "Введите AndroidId",
               cancellationToken: cancellationToken);
        }

        static async Task<Message> Usage(ITelegramBotClientWrapper botClient, Message message, IHttpSendler httpSendler, CancellationToken cancellationToken)
        {
            string responseText = "";
            if (!String.IsNullOrWhiteSpace(message.Text))
            {
                responseText = await httpSendler.SendMessagePost(message.Text, cancellationToken);
            }

            string usage = $"Usage:\n" +
                             $"/android_id - {message.Text}\n" +
                             responseText;
            return await botClient.SendTextMessageAsyncWithRemoveMarkup(
                chatId: message.Chat.Id,
                text: usage,
                cancellationToken: cancellationToken);
        }


        static Task<Message> FailingHandler(ITelegramBotClientWrapper botClient, Message message, CancellationToken cancellationToken)
        {
            throw new IndexOutOfRangeException();
        }


        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _loggerWrapper.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _loggerWrapper.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }


    }
}
