using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Wrappers
{
    public interface ILoggerWrapper<out T>
    {
        void LogInformation(string? message, params object?[] args);
        void LogError(string? message, params object?[] args);
    }
}
