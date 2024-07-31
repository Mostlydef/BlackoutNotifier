using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Wrappers;

namespace TelegramBot.Abstract
{
    public abstract class ReceiverServiceBase<TUpdateHandler>:IReceiverService
        where TUpdateHandler:IUpdateHandler
    {
        private readonly ITelegramBotClientWrapper _botClientWrapper;
        private readonly TUpdateHandler _updateHandler;
        private readonly ILoggerWrapper<ReceiverServiceBase<TUpdateHandler>> _loggerWrapper;

        internal ReceiverServiceBase(ITelegramBotClientWrapper botClient, TUpdateHandler updateHandler, ILoggerWrapper<ReceiverServiceBase<TUpdateHandler>> logger)
        {
            _botClientWrapper = botClient;
            _updateHandler = updateHandler;
            _loggerWrapper = logger;
        }

        public async Task ReceiveAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true,
            };

            var me = await _botClientWrapper.GetMeAsync(cancellationToken);
            _loggerWrapper.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

            await _botClientWrapper.ReceiveAsync( updateHandler: _updateHandler, receiverOptions: receiverOptions, cancellationToken
                : cancellationToken);
        }
    }
}
