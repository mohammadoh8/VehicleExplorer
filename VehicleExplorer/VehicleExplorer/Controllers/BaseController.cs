using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Web.Models.Api;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Controllers
{
    public class BaseController : Controller
    {

        public IActionResult CustomResponse<T>(OperationResult<T> result)
        {
            if (!result.IsSuccess || result.Data is null)
            {
                return BadRequest(ApiResponse<T>.Fail(result.ErrorMessage ?? "Request failed"));
            }

            return Ok(ApiResponse<T>.Ok(result.Data));
        }

        public IActionResult CustomModelStateResponse<T>()
        {
            var errorMessage = ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage)
                .FirstOrDefault();

            return BadRequest(ApiResponse<T>.Fail(errorMessage ?? "Invalid request"));
        }
    }
}
