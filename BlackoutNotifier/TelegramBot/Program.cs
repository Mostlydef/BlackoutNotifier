using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot.Services;

namespace TelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.Configure<BotConfiguration>(context.Configuration.GetSection(BotConfiguration.Configuration));
                    services.AddHttpClient("telegram_bot_client")
                    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                    {
                        BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                        var botToken = context.Configuration.GetSection(BotConfiguration.Configuration).Get<BotConfiguration>();
                        TelegramBotClientOptions options = new(botConfig.TelegramBotToken);
                        return new TelegramBotClient(options, httpClient);
                    });
                    services.AddScoped<UpdateHandler>();
                    services.AddScoped<ReceiverService>();
                    services.AddScoped<ITelegramBotClientFactory, TelegramBotClientFactory>();

                    services.AddHostedService<PollingService>();

                    services.AddLogging();
                })
                .Build();

            host.Run();
        }

        public class BotConfiguration
        {
            public static readonly string Configuration = "Telegram";
            public string TelegramBotToken { get; set; } = "";
            
        }
    }

}
