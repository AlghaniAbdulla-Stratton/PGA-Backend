using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Account.API.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<HttpResponseMessage> GetHttpAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return response;
        }
    }
}
