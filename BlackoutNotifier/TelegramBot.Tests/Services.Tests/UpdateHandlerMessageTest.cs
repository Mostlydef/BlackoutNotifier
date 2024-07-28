using Microsoft.Extensions.Logging;

using Moq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using TelegramBot.Networking.Interfaces;
using TelegramBot.Services;
using TelegramBot.Wrappers;


namespace TelegramBot.Tests.Services.Tests
{
    public class UpdateHandlerTextMessageTest
    {
        private readonly Mock<ITelegramBotClientWrapper> _mockBotClientWrapper;
        private readonly Mock<IHttpSendler> _mockHttpSendler;
        private readonly UpdateHandler _updateHandler;
        private readonly Mock<ITelegramBotClientFactory> _mockTelegramBotClientFactory;
        private readonly Mock<ITelegramBotClient> _mockTelegramBotClient;
        private readonly Mock<ILoggerWrapper<UpdateHandler>> _mockLoggerWrapper;

        public UpdateHandlerTextMessageTest()
        {
            _mockTelegramBotClientFactory = new Mock<ITelegramBotClientFactory>();
            _mockTelegramBotClient = new Mock<ITelegramBotClient>();
            _mockBotClientWrapper = new Mock<ITelegramBotClientWrapper>();
            _mockLoggerWrapper = new Mock<ILoggerWrapper<UpdateHandler>>();

            _mockTelegramBotClientFactory
                .Setup(x => x.CreateWrapper(_mockTelegramBotClient.Object))
                .Returns(_mockBotClientWrapper.Object);
            
            _mockHttpSendler = new Mock<IHttpSendler>();
            _updateHandler = new UpdateHandler(_mockTelegramBotClientFactory.Object, _mockLoggerWrapper.Object, _mockHttpSendler.Object);
        }

        [Fact]
        public async Task HandleUpdateAsync_ReceviesMessageResponds()
        {
            // Arrange
            long chatId = 12345;
            var messageText = "/start";
            var message = new Message
            {
                Chat = new Chat { Id = chatId },
                Text = messageText,
                Date = DateTime.UtcNow,
                MessageId = 1
            };

            var update = new Update
            {
                Id = 1,
                Message = message
            };

            _mockBotClientWrapper.Setup(x => x.SendTextMessageAsync(
                chatId,
                "Введите AndroidId", 
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Message { MessageId = 1 });

            // Act
            await _updateHandler.HandleUpdateAsync(_mockTelegramBotClient.Object, update, CancellationToken.None);

            // Assert
            _mockBotClientWrapper.Verify(x => x.SendTextMessageAsync(
                chatId,
                "Введите AndroidId", 
                It.IsAny<CancellationToken>()
            ), Times.Once);

        }

        [Fact]
        public async Task HandleUpdateAsync_BotOnMessageReceived_Usage()
        {
            // Arrange
            long chatId = 12345;
            var messageText = "12345";
            string responseMessage = $"Usage:\n" +
                     $"/android_id - 12345\n";
            var message = new Message
            {
                Chat = new Chat { Id = chatId },
                Text = messageText,
                Date = DateTime.UtcNow,
                MessageId = 1
            };

            var update = new Update
            {
                Id = 1,
                Message = message
            };

            _mockBotClientWrapper.Setup(x => x.SendTextMessageAsyncWithRemoveMarkup(
                chatId,
                responseMessage,
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Message { MessageId = 1 });

            // Act
            await _updateHandler.HandleUpdateAsync(_mockTelegramBotClient.Object, update, CancellationToken.None);

            // Assert
            _mockBotClientWrapper.Verify(x => x.SendTextMessageAsyncWithRemoveMarkup(
                chatId,
                responseMessage,
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async Task HandleUpdateAsync_BotOnMessageReceived_FailingHandler()
        {
            // Arrange
            long chatId = 12345;
            var messageText = "/throw";

            var message = new Message
            {
                Chat = new Chat { Id = chatId },
                Text = messageText,
                Date = DateTime.UtcNow,
                MessageId = 1
            };

            var update = new Update
            {
                Id = 1,
                Message = message
            };

            //Act
            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _updateHandler.HandleUpdateAsync(_mockTelegramBotClient.Object, update, CancellationToken.None));
        }

        [Fact]
        public async Task HandleUpdateAsync_UnknownUpdateHandlerAsync()
        {
            // Arrenge
            Update update = new Update();
            _mockLoggerWrapper.Setup(x => x.LogInformation("Unknown update type: {UpdateType}", update.Type));

            // Act
            await _updateHandler.HandleUpdateAsync(_mockTelegramBotClient.Object, update, CancellationToken.None);

            // Assert
            _mockLoggerWrapper.Verify(x => x.LogInformation("Unknown update type: {UpdateType}", update.Type));
        }

        [Fact]
        public async Task HandleUpdateAsync_HandlePollingErrorAsync()
        {
            //Arrange 
            var exeption = new RequestException("Request error");

            //Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await _updateHandler.HandlePollingErrorAsync(_mockTelegramBotClient.Object, exeption, CancellationToken.None);
            stopwatch.Stop();

            //Assert
            Assert.InRange(stopwatch.ElapsedMilliseconds, 2000, 3000);
        }
    }
}
