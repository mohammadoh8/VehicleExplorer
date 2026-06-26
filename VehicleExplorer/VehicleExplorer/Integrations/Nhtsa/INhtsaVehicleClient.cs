using VehicleExplorer.Web.Integrations.ExternalVehicleProvider.Dtos;

namespace VehicleExplorer.Web.Integrations.ExternalVehicleProvider
{
    public interface INhtsaVehicleClient
    {
        Task<List<MakeDto>> GetAllMakesAsync(
         CancellationToken cancellationToken = default);

        Task<List<VehicleTypeDto>> GetVehicleTypesForMakeAsync(
            int makeId,
            CancellationToken cancellationToken = default);

        Task<List<ModelDto>> GetModelsForMakeYearAndTypeAsync(
            int makeId,
            int year,
            string vehicleType,
            CancellationToken cancellationToken = default);
    }
}

