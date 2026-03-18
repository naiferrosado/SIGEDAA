using Microsoft.AspNetCore.Authorization; // librería de Autorización
using Microsoft.AspNetCore.Mvc;

namespace SIGEDAA.Controllers
{
    [Authorize] // CANDADO DE SEGURIDAD
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}