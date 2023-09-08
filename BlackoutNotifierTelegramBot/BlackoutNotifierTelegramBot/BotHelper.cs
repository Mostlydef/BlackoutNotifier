using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BlackoutNotifierTelegramBot
{
    public static class BotHelper
    {
        private const string _start = "/start";
        private const string _buttonAndroidId = "Ввести Android ID";
        private const string _buttonView = "Посмотреть";
        private const string _buttonClean = "Очистить";
        private const string _buttonSend = "Отправить";

        internal static async Task Update(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
        {
            switch(update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    var message = update.Message;
                    if(!String.IsNullOrEmpty(message.Text))
                    {
                        switch (message.Text)
                        {
                            case _start:
                                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: GetButtons());
                                break;
                            case _buttonAndroidId:
                                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: GetButtons());
                                break;
                            case _buttonView:
                                //TODO:here will be API call to get AndroidId
                                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "TODO:here will be API call to get AndroidId", replyMarkup: GetButtons());
                                break;
                            case _buttonClean:
                                //TODO:here will be API call to clear AndroidID
                                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "TODO:here will be API call to clear AndroidID", replyMarkup: GetButtons());
                                break;
                            case _buttonSend:
                                //TODO:here will be API call to send AndroidID
                                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "TODO:here will be API call to send AndroidID", replyMarkup: GetButtons());
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private static IReplyMarkup? GetButtons()
        {
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>
                    {
                        new KeyboardButton(_buttonAndroidId), new KeyboardButton(_buttonView)
                    },
                    new List<KeyboardButton>
                    {
                        new KeyboardButton(_buttonClean), new KeyboardButton(_buttonSend)
                    }
                }
            );
        }

        internal static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
