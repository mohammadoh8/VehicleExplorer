namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos
{
    public sealed class NhtsaResponse<T>
    {
        public int Count { get; set; }

        public string? Message { get; set; }

        public string? SearchCriteria { get; set; }

        public List<T> Results { get; set; } = [];
    }
}
