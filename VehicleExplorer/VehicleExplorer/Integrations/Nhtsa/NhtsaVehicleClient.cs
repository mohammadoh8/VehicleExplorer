using Microsoft.Extensions.Options;
using VehicleExplorer.Web.Integrations.ExternalVehicleProvider.Dtos;
using VehicleExplorer.Web.Integrations.IntegrationClient;
using VehicleExplorer.Web.Integrations.Nhtsa;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Integrations.ExternalVehicleProvider
{
    public class NhtsaVehicleClient : INhtsaVehicleClient
    {

        private readonly IApiHttpClient _apiClient;
        private readonly NhtsaOptions _options;

        public NhtsaVehicleClient(
            IApiHttpClient apiClient,
            IOptions<NhtsaOptions> options)
        {
            _apiClient = apiClient;
            _options = options.Value;
        }

        public async Task<OperationResult<List<MakeDto>>> GetAllMakesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _apiClient.GetAsync<NhtsaResponse<MakeDto>>(
             StringHelper.CombineUrl(_options.BaseUrl, "vehicles/getallmakes?format=json"),
             cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<MakeDto>>.Failure(
                    result.ErrorMessage!);
            }

            return OperationResult<List<MakeDto>>.Success(result.Data.Results);
        }

        public async Task<OperationResult<List<VehicleTypeDto>>> GetVehicleTypesForMakeAsync(int makeId, CancellationToken cancellationToken)
        {
            var url = StringHelper.CombineUrl(_options.BaseUrl, $"vehicles/GetVehicleTypesForMakeId/{makeId}?format=json");

            var result = await _apiClient.GetAsync<NhtsaResponse<VehicleTypeDto>>(url, cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleTypeDto>>.Failure(result.ErrorMessage!);
            }

            return OperationResult<List<VehicleTypeDto>>.Success(result.Data.Results);
        }

        public async Task<OperationResult<List<ModelDto>>> GetModelsForMakeYearAndTypeAsync(int makeId, int year, string vehicleType, CancellationToken cancellationToken)
        {
            var url = StringHelper.CombineUrl(_options.BaseUrl, $"vehicles/GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}/vehicletype/{vehicleType.EncodeString()}?format=json");

            var result = await _apiClient.GetAsync<NhtsaResponse<ModelDto>>(url, cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<ModelDto>>.Failure(result.ErrorMessage!);
            }

            return OperationResult<List<ModelDto>>.Success(result.Data.Results);
        }

    }
}
