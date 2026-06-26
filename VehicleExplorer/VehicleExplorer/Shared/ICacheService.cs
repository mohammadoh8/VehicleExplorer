namespace VehicleExplorer.Web.Shared
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> callback,
            TimeSpan expiration,
            Func<T, bool>? shouldCache = null);
    }
}
