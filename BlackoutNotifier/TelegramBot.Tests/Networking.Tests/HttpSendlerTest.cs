using Moq;
using Moq.Protected;
using System.Net;

using TelegramBot.Networking.HttpDataSendler;

namespace TelegramBot.Tests.Networking.Tests
{
    public class HttpSendlerTest
    {

        [Fact]
        public async Task SendMessagePost_ReturnsResponseContent()
        {
            // Arrange
            string message = "Test message";
            CancellationToken cancellationToken = CancellationToken.None;

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(message)
                });
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var httpSendler = new HttpSendler(httpClient);

            // Act
            var responseContent = await httpSendler.SendMessagePost(message, cancellationToken);

            // Assert
            Assert.Equal(message, responseContent);
        }
         
    }
}
