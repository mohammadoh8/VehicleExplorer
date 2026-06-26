using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Integrations.IntegrationClient
{
    public interface IApiHttpClient
    {
        Task<OperationResult<T>> GetAsync<T>(string requestUri,CancellationToken cancellationToken = default);
    }
}
