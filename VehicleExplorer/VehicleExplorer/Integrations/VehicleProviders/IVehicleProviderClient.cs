using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Integrations.VehicleProviders
{
    public interface IVehicleProviderClient
    {
        Task<OperationResult<List<VehicleMake>>> GetAllMakesAsync(
            CancellationToken cancellationToken = default);

        Task<OperationResult<List<VehicleType>>> GetVehicleTypesForMakeAsync(
            int makeId,
            CancellationToken cancellationToken = default);

        Task<OperationResult<List<VehicleModel>>> GetModelsForMakeYearAndTypeAsync(
            int makeId,
            int year,
            string vehicleType,
            CancellationToken cancellationToken = default);
    }
}
