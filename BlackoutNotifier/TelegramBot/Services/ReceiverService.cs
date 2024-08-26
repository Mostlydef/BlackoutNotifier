using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBot.Abstract;
using TelegramBot.Wrappers;

namespace TelegramBot.Services
{
    public class ReceiverService : ReceiverServiceBase<UpdateHandler>
    {

        public ReceiverService(
       ITelegramBotClientWrapper botClient,
       UpdateHandler updateHandler,
       ILoggerWrapper<ReceiverServiceBase<UpdateHandler>> logger)
       : base(botClient, updateHandler, logger)
        {
        }
    }
}

