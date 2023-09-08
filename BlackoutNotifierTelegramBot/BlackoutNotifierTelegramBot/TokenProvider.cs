using Microsoft.Extensions.Options;
using BlackoutNotifierTelegramBot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BlackoutNotifierTelegramBot
{
    internal class TokenProvider: ITokenProvider
    {
        private readonly TelegramBotDetails _telegramBotDetails;
        private readonly TelegramBotClient _telegramBotClient;

        public TokenProvider(IOptions<TelegramBotDetails> telegramBotDetails)
        {
            _telegramBotDetails = telegramBotDetails.Value;
            _telegramBotClient = new TelegramBotClient(_telegramBotDetails.TelegramBotToken);
        }

        void ITokenProvider.GetToken()
        {
            _telegramBotClient.StartReceiving(BotHelper.Update, BotHelper.Error, null);
        }
    }
}
