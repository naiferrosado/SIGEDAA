using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Linq;
using System.Threading.Tasks;

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

       
        // INDEX (Lista)
     
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

      
        // CREATE (Crear)
      
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
            // Le decimos a C# que no exija un Club al crear al entrenador
            ModelState.Remove("Club");

            if (ModelState.IsValid)
            {
                // Por defecto, al crear un entrenador, debe estar activo
                entrenador.EstadoActivo = true;

                _context.Add(entrenador);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Entrenador registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubes = new SelectList(
                _context.Clubes.OrderBy(c => c.NombreClub).ToList(),
                "IdClub",
                "NombreClub"
            );

            return View(entrenador);
        }

        
        // DETAILS (Ver Detalles)
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entrenador = await _context.Entrenadores
                .FirstOrDefaultAsync(m => m.IdEntrenador == id);

            if (entrenador == null) return NotFound();

            return View(entrenador);
        }

      
        // EDIT (Editar e Inhabilitar)
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entrenador = await _context.Entrenadores.FindAsync(id);

            if (entrenador == null) return NotFound();

            ViewBag.Clubes = new SelectList(
                _context.Clubes.OrderBy(c => c.NombreClub).ToList(),
                "IdClub",
                "NombreClub"
            );

            return View(entrenador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Entrenador entrenador)
        {
            if (id != entrenador.IdEntrenador) return NotFound();

            // Quitamos la validación del club igual que en Create
            ModelState.Remove("Club");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrenador);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Datos del entrenador actualizados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntrenadorExists(entrenador.IdEntrenador))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.Clubes = new SelectList(
                _context.Clubes.OrderBy(c => c.NombreClub).ToList(),
                "IdClub",
                "NombreClub"
            );

            return View(entrenador);
        }

        // MÉTODO AUXILIAR
        
        private bool EntrenadorExists(int id)
        {
            return _context.Entrenadores.Any(e => e.IdEntrenador == id);
        }
    }
}