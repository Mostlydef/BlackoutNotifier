﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Abstract
{
    public interface IReceiverService
    {
        Task ReceiveAsync(CancellationToken cancellationToken);
    }
}
