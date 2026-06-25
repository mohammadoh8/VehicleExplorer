using Microsoft.AspNetCore.Mvc;

namespace VehicleExplorer.Controllers
{
    public class VehiclesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
