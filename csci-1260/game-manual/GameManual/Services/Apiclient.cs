using System.Net.Http.Json;

namespace GameManual.Services
{
    public class ApiClient
    {
        private readonly IHttpClientFactory _factory;
        public ApiClient(IHttpClientFactory factory) => _factory = factory;

        public async Task<T?> GetAsync<T>(string path, CancellationToken ct = default)
        {
            var client = _factory.CreateClient("PokeApi");
            return await client.GetFromJsonAsync<T>(path, ct);
        }
    }
}

