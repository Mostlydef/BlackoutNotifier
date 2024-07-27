using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using TelegramBot.Networking.Interfaces;
using TelegramBot.Services;
using Microsoft.VisualBasic;
using Telegram.Bot.Types.ReplyMarkups;


namespace TelegramBot.Tests.Services.Tests
{
    public class UpdateHandlerTextMessageTest
    {
        private readonly Mock<ITelegramBotClient> _mockBotClient;
        private readonly Mock<ILogger<UpdateHandler>> _mockLogger;
        private readonly Mock<IHttpSendler> _mockHttpSendler;
        private readonly UpdateHandler _updateHandler;

        public UpdateHandlerTextMessageTest()
        {
            _mockBotClient = new Mock<ITelegramBotClient>();
            _mockLogger = new Mock<ILogger<UpdateHandler>>();
            _mockHttpSendler = new Mock<IHttpSendler>();
            _updateHandler = new UpdateHandler(_mockBotClient.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task HandleUpdateAsync_ReceviesMessageResponds()
        {
            // Arrange
            var chatId = 12345;
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

            _mockBotClient.Setup(x => x.SendTextMessageAsync(
                It.Is<ChatId>(id => id.Identifier == chatId),
                "Введите AndroidId",
                null,
                null,
                null,
                false,
                false,
                false,
                null,
                null,
                null,
                CancellationToken.None
            )).ReturnsAsync(new Message { MessageId = 1 });

            // Act
            await _updateHandler.HandleUpdateAsync(_mockBotClient.Object, update, CancellationToken.None);

            // Assert
            _mockBotClient.Verify(x => x.SendTextMessageAsync(
                It.Is<ChatId>(id => id.Identifier == chatId),
                "Введите AndroidId",
                null,
                null,
                null,
                false,
                false,
                false,
                null,
                false,
                null,
                CancellationToken.None
            ), Times.Once);

        }
    }
}
