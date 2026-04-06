using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class EntrenadoresController : Controller
    {
        private readonly AppDbContext _context;

        public EntrenadoresController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var entrenadores = await _context.Entrenadores
                .Include(e => e.Club)
                .OrderBy(e => e.Apellidos)
                .ToListAsync();

            return View(entrenadores);
        }

        public IActionResult Create()
        {
            ViewBag.Clubes = new SelectList(
                _context.Clubes.OrderBy(c => c.NombreClub).ToList(),
                "IdClub",
                "NombreClub"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Entrenador entrenador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entrenador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubes = new SelectList(
                _context.Clubes.OrderBy(c => c.NombreClub).ToList(),
                "IdClub",
                "NombreClub"
            );

            return View(entrenador);
        }
    }
}