using Microsoft.Extensions.Options;
using VehicleExplorer.Web.Integrations.IntegrationClient;
using VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa.Dtos;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa
{
    public class NhtsaVehicleClient : IVehicleProviderClient
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

        public async Task<OperationResult<List<VehicleMake>>> GetAllMakesAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _apiClient.GetAsync<NhtsaResponse<NhtsaMakeDto>>(
                StringHelper.CombineUrl(_options.BaseUrl, "vehicles/getallmakes?format=json"),
                cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleMake>>.Failure(result.ErrorMessage!);
            }

            var makes = result.Data.Results.Select(NhtsaMakeDto.ToVehicleMake)
                .ToList();

            return OperationResult<List<VehicleMake>>.Success(makes);
        }

        public async Task<OperationResult<List<VehicleType>>> GetVehicleTypesForMakeAsync(
            int makeId,
            CancellationToken cancellationToken)
        {
            var url = StringHelper.CombineUrl(
                _options.BaseUrl,
                $"vehicles/GetVehicleTypesForMakeId/{makeId}?format=json");

            var result = await _apiClient.GetAsync<NhtsaResponse<NhtsaVehicleTypeDto>>(
                url,
                cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleType>>.Failure(result.ErrorMessage!);
            }

            var vehicleTypes = result.Data.Results.Select(NhtsaVehicleTypeDto.ToVehicleType).ToList();

            return OperationResult<List<VehicleType>>.Success(vehicleTypes);
        }

        public async Task<OperationResult<List<VehicleModel>>> GetModelsForMakeYearAndTypeAsync(
            int makeId,
            int year,
            string vehicleType,
            CancellationToken cancellationToken)
        {
            var url = StringHelper.CombineUrl(
                _options.BaseUrl,
                $"vehicles/GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}/vehicletype/{vehicleType.EncodeString()}?format=json");

            var result = await _apiClient.GetAsync<NhtsaResponse<NhtsaModelDto>>(
                url,
                cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleModel>>.Failure(result.ErrorMessage!);
            }

            var models = result.Data.Results.Select(NhtsaModelDto.ToVehicleModel).ToList();

            return OperationResult<List<VehicleModel>>.Success(models);
        }
    }
}
