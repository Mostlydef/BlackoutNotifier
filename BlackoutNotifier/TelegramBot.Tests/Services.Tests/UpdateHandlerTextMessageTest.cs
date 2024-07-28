using Microsoft.Extensions.Logging;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Networking.Interfaces;
using TelegramBot.Services;


namespace TelegramBot.Tests.Services.Tests
{
    public class UpdateHandlerTextMessageTest
    {
        private readonly Mock<ITelegramBotClientWrapper> _mockBotClientWrapper;
        private readonly Mock<ILogger<UpdateHandler>> _mockLogger;
        private readonly Mock<IHttpSendler> _mockHttpSendler;
        private readonly UpdateHandler _updateHandler;
        private readonly Mock<ITelegramBotClientFactory> _mockTelegramBotClientFactory;
        private readonly Mock<ITelegramBotClient> _mockTelegramBotClient;

        public UpdateHandlerTextMessageTest()
        {
            _mockTelegramBotClientFactory = new Mock<ITelegramBotClientFactory>();
            _mockTelegramBotClient = new Mock<ITelegramBotClient>();
            _mockBotClientWrapper = new Mock<ITelegramBotClientWrapper>();

            _mockTelegramBotClientFactory
                .Setup(x => x.CreateWrapper(_mockTelegramBotClient.Object))
                .Returns(_mockBotClientWrapper.Object);
            
            _mockLogger = new Mock<ILogger<UpdateHandler>>();
            _mockHttpSendler = new Mock<IHttpSendler>();
            _updateHandler = new UpdateHandler(_mockTelegramBotClientFactory.Object, _mockLogger.Object);
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
            var mockBotClient = new Mock<ITelegramBotClient>();
            var mockLogger = new Mock<ILogger<UpdateHandler>>();
            var udateHandler = new UpdateHandler(mockBotClient.Object, mockLogger.Object);

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
    }
}
