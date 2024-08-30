using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

namespace TestBotTG
{
    class Program
    {
        static void Main(string[] args)
        {
            var botClient = new TelegramBotClient("7345391263:AAEsx-jHCDrX70ggxm8yv6kCqywNn7EfNIo");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                cancellationToken: cancellationToken
            );

            Console.WriteLine("Bot is running...");
            Console.ReadLine();
            cts.Cancel(); // Остановка бота
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null && update.Message.Text != null)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"You said: {update.Message.Text}", cancellationToken: cancellationToken);
            }
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}