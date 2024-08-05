using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Networking.Interfaces
{
    public interface IHttpSendler
    {
        Task<string> SendMessagePost(string message, CancellationToken cancellationToken);
    }
}
