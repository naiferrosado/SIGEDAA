using Microsoft.AspNetCore.Mvc;

namespace SIGEDAA.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}