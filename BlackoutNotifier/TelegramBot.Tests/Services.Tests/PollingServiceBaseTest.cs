using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot.Abstract;
using TelegramBot.Networking.Interfaces;
using TelegramBot.Services;
using TelegramBot.Wrappers;

namespace TelegramBot.Tests.Services.Tests
{
    public class PollingServiceBaseTest
    {
        private readonly PollingService _pollingService;
        private readonly UpdateHandler _updateHandler;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<ILoggerWrapper<PollingService>> _mockILoggerWrapper;
        private readonly Mock<ITelegramBotClient> _mockTelegramBotClient;
        private readonly Mock<ITelegramBotClientFactory> _mockTelegramBotClientFactory;
        private readonly Mock<ITelegramBotClientWrapper> _mockTelegramBotClientWrapper;
        private readonly Mock<IHttpSendler> _mockHttpSendler;
        private readonly Mock<ILoggerWrapper<UpdateHandler>> _mockLoggerWrapper;

        public PollingServiceBaseTest()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockILoggerWrapper = new Mock<ILoggerWrapper<PollingService>>();
            _mockTelegramBotClientFactory = new Mock<ITelegramBotClientFactory>();
            _mockTelegramBotClientWrapper = new Mock<ITelegramBotClientWrapper>();
            _mockTelegramBotClient = new Mock<ITelegramBotClient>();
            _mockLoggerWrapper= new Mock<ILoggerWrapper<UpdateHandler>>();
            _mockHttpSendler = new Mock<IHttpSendler>();

            _mockTelegramBotClientFactory
               .Setup(x => x.CreateWrapper(_mockTelegramBotClient.Object))
               .Returns(_mockTelegramBotClientWrapper.Object);


            _updateHandler = new UpdateHandler(_mockTelegramBotClientFactory.Object, _mockLoggerWrapper.Object, _mockHttpSendler.Object);
            _pollingService = new PollingService(_mockServiceProvider.Object, _mockILoggerWrapper.Object);
        }

        [Fact]
        public async Task ExecuteAsyncTest()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();

            // Act 
            var executeTask = _pollingService.StartAsync(It.IsAny<CancellationToken>());
            cancellationTokenSource.Cancel();
            await executeTask;

            // Assert
            _mockILoggerWrapper.Verify(x => x.LogInformation(
                "Starting polling service"));
        }

        [Fact]
        public async Task DoWorkTest()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var executeTask = _pollingService.StartAsync(It.IsAny<CancellationToken>());
            await Task.Delay(100);
            cancellationTokenSource.Cancel();
            await executeTask;

            // Assert
            _mockTelegramBotClientWrapper
                .Setup(x => x.ReceiveAsync(_updateHandler, It.IsAny<ReceiverOptions>(), It.IsAny<CancellationToken>()));

        }

        [Fact]
        public async Task DoWorkLogErrorException()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            _mockTelegramBotClientWrapper
                .Setup(x => x.ReceiveAsync(_updateHandler, It.IsAny<ReceiverOptions>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Test"));

            // Act
            var doWork = _pollingService.StartAsync(cancellationToken);

            await Task.Delay(100);
            cancellationTokenSource.Cancel();
            await doWork;

            // Assert
            _mockILoggerWrapper.Verify(x => x.LogError(
                "Polling failed with exception: {Exception}",
                It.IsAny<Exception>()));


        }
    }
}
