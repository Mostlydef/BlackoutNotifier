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

namespace TelegramBot.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly IHttpSendler _httpSendler;


        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger)
        {
            _botClient = botClient;
            _logger = logger;
            _httpSendler = new HttpSendler(new HttpClient());
        }
        public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
        {
            if(update.Type == UpdateType.Message && update.Message != null)
            {
                await BotOnMessageReceived(update.Message, cancellationToken);
            }
            await UnknownUpdateHandlerAsync(update, cancellationToken);
        }

        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            string messageText;
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (String.IsNullOrWhiteSpace(message.Text))
                return;
            else
                messageText = message.Text;
            Message sentMessage;
            switch (messageText)
            {
                case "/android_id":
                    sentMessage = await OnStart(_botClient, message, cancellationToken);
                    break;
                case "/throw":
                    sentMessage = await FailingHandler(_botClient, message, cancellationToken);
                    break;
                case "/start":
                    sentMessage = await OnStart(_botClient, message, cancellationToken);
                    break;
                default:
                    sentMessage = await Usage(_botClient, message, _httpSendler, cancellationToken);
                    break;
            }

            _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);


            static async Task<Message> OnStart(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: "Введите AndroidId",
                   cancellationToken: cancellationToken);
            }

            static async Task<Message> Usage(ITelegramBotClient botClient, Message message, IHttpSendler httpSendler, CancellationToken cancellationToken)
            {
                string responseText = "";
                if (!String.IsNullOrWhiteSpace(message.Text))
                {
                    responseText = await httpSendler.SendMessagePost(message.Text, cancellationToken);
                }

                string usage = $"Usage:\n" +
                                 $"/android_id - {message.Text}\n" +
                                 responseText;
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }


            static Task<Message> FailingHandler(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                throw new IndexOutOfRangeException();
            }

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
