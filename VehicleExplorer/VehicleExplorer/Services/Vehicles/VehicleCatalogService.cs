using FluentValidation;
using VehicleExplorer.Web.Integrations.VehicleProviders;
using VehicleExplorer.Web.Services.Vehicles.Requests;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Services.Vehicles
{
    public sealed class VehicleCatalogService : IVehicleCatalogService
    {
        private readonly IVehicleProviderClient _vehicleClient;
        private readonly ICacheService _cacheService;
        private readonly IValidator<VehicleModelsRequest> _vehicleModelsValidator;

        public VehicleCatalogService(
            IVehicleProviderClient vehicleClient,
            ICacheService cacheService,
            IValidator<VehicleModelsRequest> vehicleModelsValidator)
        {
            _vehicleClient = vehicleClient;
            _cacheService = cacheService;
            _vehicleModelsValidator = vehicleModelsValidator;
        }

        public async Task<OperationResult<List<VehicleMake>>> GetMakesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _cacheService.GetOrCreateAsync(
                "vehicle-makes",() => LoadMakesAsync(cancellationToken),
                TimeSpan.FromHours(24));
        }

        private async Task<OperationResult<List<VehicleMake>>> LoadMakesAsync(
            CancellationToken cancellationToken)
        {
            var result = await _vehicleClient.GetAllMakesAsync(cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleMake>>.Failure(
                    result.ErrorMessage!);
            }

            var makes = result.Data.OrderBy(make => make.Name).ToList();

            return OperationResult<List<VehicleMake>>.Success(makes);
        }

        public async Task<OperationResult<List<VehicleType>>> GetVehicleTypesAsync(
            int makeId,
            CancellationToken cancellationToken = default)
        {
            if (makeId <= 0)
            {
                return OperationResult<List<VehicleType>>.Failure("MakeId must be bigger than 0");
            }

            var result = await _vehicleClient.GetVehicleTypesForMakeAsync(makeId, cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleType>>.Failure(
                    result.ErrorMessage!);
            }

            var vehicleTypes = result.Data.OrderBy(type => type.Name).ToList();

            return OperationResult<List<VehicleType>>.Success(vehicleTypes);
        }

        public async Task<OperationResult<List<VehicleModel>>> GetModelsAsync(VehicleModelsRequest request,CancellationToken cancellationToken = default)
        {
           
            var validationResult = await _vehicleModelsValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return OperationResult<List<VehicleModel>>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            var result = await _vehicleClient.GetModelsForMakeYearAndTypeAsync(
                request.MakeId,
                request.Year,
                request.VehicleType,
                cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return OperationResult<List<VehicleModel>>.Failure(
                    result.ErrorMessage!);
            }

            var models = result.Data.OrderBy(model => model.Name).ToList();

            return OperationResult<List<VehicleModel>>.Success(models);
        }
    }
}
