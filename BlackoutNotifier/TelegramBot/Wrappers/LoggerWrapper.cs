using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Wrappers
{
    public class LoggerWrapper<T> : ILoggerWrapper<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerWrapper(ILogger<T> logger) 
        {
            _logger = logger;
        }

        public void LogError(string? message, params object?[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogInformation(string? message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }
    }
}
