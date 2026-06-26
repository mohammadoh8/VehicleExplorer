using System.Net;
using System.Net.Http;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VehicleExplorer.Web.Integrations.IntegrationClient
{
    public class ApiHttpClient : IApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiHttpClient> _logger;
        public ApiHttpClient(HttpClient httpClient,ILogger<ApiHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ApiClientResult<T>> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "External API request failed. Uri: {RequestUri}, StatusCode: {StatusCode}",
                    requestUri,
                    response.StatusCode);

                return ApiClientResult<T>.Failure("The external service is currently unavailable.",response.StatusCode);
                
            }

            await using var stringStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var data = await JsonSerializer.DeserializeAsync<T>(stringStream);

            if (data is null)
            {
                ApiClientResult<T>.Failure("The external service is currently unavailable.", response.StatusCode);
            }
            return ApiClientResult<T>.Success(data!, response.StatusCode);
        }
    }
}
