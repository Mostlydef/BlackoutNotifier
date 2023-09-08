// See https://aka.ms/new-console-template for more information
using BlackoutNotifierTelegramBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                    devEnvironmentVariable.ToLower() == "development";


var builder = new ConfigurationBuilder();

builder
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


if (isDevelopment)
{
    builder.AddUserSecrets<Program>();
}

IConfigurationRoot configuration = builder.Build();
IServiceCollection services = new ServiceCollection();

services
    .Configure<TelegramBotDetails>(configuration.GetSection(nameof(TelegramBotDetails)))
    .AddOptions()
    .AddLogging()
    .AddSingleton<ITokenProvider, TokenProvider>()
    .BuildServiceProvider();

var serviceProvider = services.BuildServiceProvider();

var provider = serviceProvider.GetService<ITokenProvider>();

provider.GetToken();

Console.ReadLine();
