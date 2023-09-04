// See https://aka.ms/new-console-template for more information
using BlackoutNotifierTelegramBot;
using Telegram.Bot;
using Telegram.Bot.Types;

var client = new TelegramBotClient("6683922317:AAEEtP7hLK_LFOUAcQW4ReKXCkoxhGaaR4s");
client.StartReceiving(BotHelper.Update, BotHelper.Error, null);
Console.ReadLine();
Console.WriteLine("Hello, World!");
