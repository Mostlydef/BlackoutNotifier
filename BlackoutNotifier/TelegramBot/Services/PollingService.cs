using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Abstract;

namespace TelegramBot.Services
{
    public class PollingService : PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
        {
        }
    }
}
