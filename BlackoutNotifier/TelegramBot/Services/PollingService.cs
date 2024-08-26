using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Abstract;
using TelegramBot.Wrappers;

namespace TelegramBot.Services
{
    public class PollingService : PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider serviceProvider, ILoggerWrapper<PollingService> logger)
        : base(serviceProvider, logger)
        {
        }
    }
}
