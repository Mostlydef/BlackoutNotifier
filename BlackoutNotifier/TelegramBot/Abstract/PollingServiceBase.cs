using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Wrappers;

namespace TelegramBot.Abstract
{
    public abstract class PollingServiceBase<TReceiverService>:BackgroundService
        where TReceiverService : IReceiverService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerWrapper<PollingServiceBase<TReceiverService>> _loggerWrapper;

        internal PollingServiceBase(
        IServiceProvider serviceProvider,
        ILoggerWrapper<PollingServiceBase<TReceiverService>> logger)
        {
            _serviceProvider = serviceProvider;
            _loggerWrapper = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _loggerWrapper.LogInformation("Starting polling service");

            await DoWork(cancellationToken);
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();

                    await receiver.ReceiveAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _loggerWrapper.LogError("Polling failed with exception: {Exception}", ex);

                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }

    }


}
