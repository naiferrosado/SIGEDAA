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
                .OrderBy(e => e.Apellidos)
                .ToArrayAsync();

            // Agrupamos por ID del entrenador en caso de que dirijan más de un club
            var clubesDirigidos = await _context.Clubes
                .Where(c => c.IdEntrenadorPrincipal != null)
                .GroupBy(c => c.IdEntrenadorPrincipal.Value)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => string.Join(" y ", g.Select(c => c.NombreClub))
                );

            ViewBag.ClubesDirigidos = clubesDirigidos;

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
        {// Le decimos a C# que no exija un Club al crear al entrenador
            ModelState.Remove("Club");
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