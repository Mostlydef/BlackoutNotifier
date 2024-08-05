using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TelegramBot.Networking.Interfaces;

namespace TelegramBot.Networking.HttpDataSendler
{
    public class HttpSendler : IHttpSendler
    {
        private readonly HttpClient _httpClient;
        private const string Url = "https://localhost:5001/data";

        public HttpSendler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendMessagePost(string message, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, Url);
            request.Content = new StringContent(message);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
