using System.Text.Json;
using VehicleExplorer.Web.Shared;

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

        public async Task<OperationResult<T>> GetAsync<T>(string requestUrl, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "External API request failed. Uri: {RequestUri}, StatusCode: {StatusCode}",
                    requestUrl,
                    response.StatusCode);

                return OperationResult<T>.Failure("The external service is currently unavailable");
                
            }

            await using var stringStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var data = await JsonSerializer.DeserializeAsync<T>(stringStream);

            if (data is null)
            {
                return OperationResult<T>.Failure("The external service is currently unavailable");
            }
            return OperationResult<T>.Success(data!);
        }
    }
}
