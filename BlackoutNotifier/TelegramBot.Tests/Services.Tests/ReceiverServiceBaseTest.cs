using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using TelegramBot.Abstract;
using TelegramBot.Services;
using Moq;
using TelegramBot.Wrappers;
using Telegram.Bot.Types.Enums;
using TelegramBot.Networking.Interfaces;

namespace TelegramBot.Tests.Services.Tests
{
    public class ReceiverServiceBaseTest
    {
        private readonly ReceiverService _receiverService;
        private readonly UpdateHandler _updateHandler;
        private readonly Mock<ITelegramBotClientFactory> _mockTelegramBotClientFactory;
        private readonly Mock<IHttpSendler> _mockHttpSendler;
        private readonly Mock<ILoggerWrapper<UpdateHandler>> _mockloggerWrapperForUpdateHandle;
        private readonly Mock<ITelegramBotClient> _mockTelegramBotClient;
        private readonly Mock<ITelegramBotClientWrapper> _mockTelegramBotClientWrapper;
        private readonly Mock<ILoggerWrapper<ReceiverServiceBase<UpdateHandler>>> _mockLoggerWrapper;


        public ReceiverServiceBaseTest()
        {
            _mockLoggerWrapper = new Mock<ILoggerWrapper<ReceiverServiceBase<UpdateHandler>>>();
            _mockTelegramBotClientWrapper = new Mock<ITelegramBotClientWrapper>();
            _mockTelegramBotClient = new Mock<ITelegramBotClient>();
            _mockTelegramBotClientFactory = new Mock<ITelegramBotClientFactory>();
            _mockloggerWrapperForUpdateHandle = new Mock<ILoggerWrapper<UpdateHandler>>();
            _mockHttpSendler = new Mock<IHttpSendler>();

            _mockTelegramBotClientFactory
                .Setup(x => x.CreateWrapper(_mockTelegramBotClient.Object))
                .Returns(_mockTelegramBotClientWrapper.Object);

            _updateHandler = new UpdateHandler(_mockTelegramBotClientFactory.Object, _mockloggerWrapperForUpdateHandle.Object, _mockHttpSendler.Object);
            _receiverService = new ReceiverService(_mockTelegramBotClientWrapper.Object, _updateHandler, _mockLoggerWrapper.Object);
        }

        [Fact]
        public async Task ReceiverServiceBase_ReceiveAsync()
        {
            // Arrange

            _mockTelegramBotClientWrapper.Setup(x => x.GetMeAsync(
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new Telegram.Bot.Types.User { Username = "Test" } );

            _mockTelegramBotClientWrapper.Setup(x => x.ReceiveAsync(
                It.IsAny<UpdateHandler>(),
                It.IsAny<ReceiverOptions>(),
                It.IsAny<CancellationToken>()
                )).Returns(Task.CompletedTask);
                

            // Act
            await _receiverService.ReceiveAsync(CancellationToken.None);

            // Assert
            _mockTelegramBotClientWrapper.Verify(x => x.ReceiveAsync(
                 It.IsAny<UpdateHandler>(),
                It.IsAny<ReceiverOptions>(),
                CancellationToken.None));
        }
    }
}
