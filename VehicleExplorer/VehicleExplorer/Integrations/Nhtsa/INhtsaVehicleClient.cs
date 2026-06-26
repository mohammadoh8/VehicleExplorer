using VehicleExplorer.Web.Integrations.ExternalVehicleProvider.Dtos;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Integrations.ExternalVehicleProvider
{
    public interface INhtsaVehicleClient
    {
        Task<OperationResult<List<MakeDto>>> GetAllMakesAsync(
         CancellationToken cancellationToken = default);

        Task<OperationResult<List<VehicleTypeDto>>> GetVehicleTypesForMakeAsync(
            int makeId,
            CancellationToken cancellationToken = default);

        Task<OperationResult<List<ModelDto>>> GetModelsForMakeYearAndTypeAsync(
            int makeId,
            int year,
            string vehicleType,
            CancellationToken cancellationToken = default);
    }
}

