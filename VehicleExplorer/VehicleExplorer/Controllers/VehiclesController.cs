using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Web.Controllers;
using VehicleExplorer.Web.Integrations.VehicleProviders;
using VehicleExplorer.Web.Models.Api;
using VehicleExplorer.Web.Services.Vehicles;
using VehicleExplorer.Web.Services.Vehicles.Requests;

namespace VehicleExplorer.Controllers
{
    public class VehiclesController : BaseController
    {
        private readonly IVehicleCatalogService _vehicleCatalogService;

        public VehiclesController(IVehicleCatalogService vehicleCatalogService)
        {
            _vehicleCatalogService = vehicleCatalogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Makes(
            [FromQuery] VehicleMakesRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return CustomModelStateResponse<PagedResponse<VehicleMake>>();
            }

            var result = await _vehicleCatalogService.GetMakesAsync(request, cancellationToken);

            return CustomResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> Types(int makeId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return CustomModelStateResponse<List<VehicleType>>();
            }

            var result = await _vehicleCatalogService.GetVehicleTypesAsync(makeId, cancellationToken);

            return CustomResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> Models(
            [FromQuery] VehicleModelsRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return CustomModelStateResponse<List<VehicleModel>>();
            }

            var result = await _vehicleCatalogService.GetModelsAsync(request, cancellationToken);

            return CustomResponse(result);
        }
    }
}
