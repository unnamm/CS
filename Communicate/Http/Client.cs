using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicate.Http
{
    public class Client
    {
        private readonly HttpClient _client = new();

        public async Task<HttpResponseMessage> SendAsync(string url, Dictionary<string, string> headers)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            foreach (var pair in headers)
            {
                request.Headers.Add(pair.Key, pair.Value);
            }
            var response = await _client.SendAsync(request);
            return response.EnsureSuccessStatusCode();
        }

        public Task<HttpResponseMessage> SendAsync(string url, string headerKey, string headerValue) => SendAsync(url, [(headerKey, headerValue)]);
        public Task<HttpResponseMessage> SendAsync(string url, params (string key, string value)[] pairs) => SendAsync(url, pairs.ToDictionary());

        //if .net8
        //public Task<HttpResponseMessage> SendAsync(string url, params (string key, string value)[] pairs)
        //    => SendAsync(url, pairs.ToDictionary(h => h.key, h => h.value));
    }
}
