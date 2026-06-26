using System.Text.Json;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Integrations.IntegrationClient
{
    public class ApiHttpClient : IApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiHttpClient> _logger;
        public ApiHttpClient(HttpClient httpClient, ILogger<ApiHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<OperationResult<T>> GetAsync<T>(string requestUrl, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync(requestUrl, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "external API request failed Uri: {RequestUri}", requestUrl);

                return OperationResult<T>.Failure("The external service request failed");
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("external API request failed Uri: {RequestUri}, StatusCode: {StatusCode}",
                    requestUrl,
                    response.StatusCode);

                return OperationResult<T>.Failure("The external service request failed");
            }

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            var data = JsonSerializer.Deserialize<T>(responseString);

            if (data is null)
            {
                return OperationResult<T>.Failure("The external service request returned empty data");
            }

            return OperationResult<T>.Success(data);

        }
    }
}
