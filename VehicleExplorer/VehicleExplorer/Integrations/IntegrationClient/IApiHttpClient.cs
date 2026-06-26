namespace VehicleExplorer.Web.Integrations.IntegrationClient
{
    public interface IApiHttpClient
    {
        Task<ApiClientResult<T>> GetAsync<T>(string requestUri,CancellationToken cancellationToken = default);
    }
}
