
using VehicleExplorer.Web.Integrations.ExternalVehicleProvider.Dtos;

namespace VehicleExplorer.Web.Integrations.ExternalVehicleProvider
{
    public class NhtsaVehicleClient : INhtsaVehicleClient
    {
        public Task<List<MakeDto>> GetAllMakesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ModelDto>> GetModelsForMakeYearAndTypeAsync(int makeId, int year, string vehicleType, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<VehicleTypeDto>> GetVehicleTypesForMakeAsync(int makeId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
