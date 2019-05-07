using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Utils.Json;

namespace Crm.Utils.Http
{
    public static class HttpClientExtensions
    {
        public static async Task GetAsync(this IHttpClientFactory factory, string uri, object parameters = default,
            CancellationToken ct = default)
        {
            var fullUri = GetFullUri(uri, parameters);

            using (var client = factory.CreateClient())
            {
                var result = await client.GetAsync(fullUri, ct).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    GenerateException(fullUri, result);
                }
            }
        }

        public static async Task<TResponse> GetAsync<TResponse>(this IHttpClientFactory factory, string uri,
            object parameters = default, CancellationToken ct = default)
        {
            var fullUri = GetFullUri(uri, parameters);

            using (var client = factory.CreateClient())
            {
                var result = await client.GetAsync(fullUri, ct).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    GenerateException(fullUri, result);
                }

                var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                return content.FromJsonString<TResponse>();
            }
        }

        public static async Task PostAsync(this IHttpClientFactory factory, string uri, object body = default,
            CancellationToken ct = default)
        {
            var fullUri = GetFullUri(uri);

            using (var client = factory.CreateClient())
            {
                var result = await client.PostAsync(fullUri, body.ToJsonStringContent(), ct).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    GenerateException(fullUri, result);
                }
            }
        }

        public static async Task<TResponse> PostAsync<TResponse>(this IHttpClientFactory factory, string uri,
            object body = default, CancellationToken ct = default)
        {
            var fullUri = GetFullUri(uri);

            using (var client = factory.CreateClient())
            {
                var result = await client.PostAsync(fullUri, body.ToJsonStringContent(), ct).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    GenerateException(fullUri, result);
                }

                var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                return content.FromJsonString<TResponse>();
            }
        }

        private static string GetFullUri(string uri, object parameters = null)
        {
            return $"{uri}{parameters.ToQueryParams()}";
        }

        private static void GenerateException(string uri, HttpResponseMessage message)
        {
            throw new HttpRequestException(
                $"Request to {uri} failed. Status code: {message.StatusCode}, Reason: {message.ReasonPhrase}");
        }
    }
}