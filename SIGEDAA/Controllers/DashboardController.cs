using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEDAA.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AtletasCount = await _context.Atletas.CountAsync();
            ViewBag.EquiposCount = await _context.Equipos.CountAsync();
            ViewBag.TorneosCount = await _context.Competencias.CountAsync();
            ViewBag.ResultadosCount = await _context.ResultadosCompetencia.CountAsync();

            return View();
        }
    }
}