using VehicleExplorer.Web.Shared;
using VehicleExplorer.Web.Integrations.VehicleProviders;
using VehicleExplorer.Web.Services.Vehicles.Requests;

namespace VehicleExplorer.Web.Services.Vehicles
{
    public interface IVehicleCatalogService
    {
        Task<OperationResult<List<VehicleMake>>> GetMakesAsync(
            CancellationToken cancellationToken = default);

        Task<OperationResult<List<VehicleType>>> GetVehicleTypesAsync(
            int makeId,
            CancellationToken cancellationToken = default);

        Task<OperationResult<List<VehicleModel>>> GetModelsAsync(
            VehicleModelsRequest request,
            CancellationToken cancellationToken = default);
    }
}
