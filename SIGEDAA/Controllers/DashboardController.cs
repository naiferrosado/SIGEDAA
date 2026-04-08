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
        // SIN private readonly
        public AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Contamos los registros reales en la base de datos
            ViewBag.AtletasCount = await _context.Atletas.CountAsync();

            // Reemplazamos Equipos por Clubes
            ViewBag.ClubesCount = await _context.Clubes.CountAsync();

            ViewBag.TorneosCount = await _context.Competencias.CountAsync();
            ViewBag.ResultadosCount = await _context.ResultadosCompetencia.CountAsync();

            return View();
        }
    }
}