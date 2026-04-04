using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class ClubesController : Controller
    {
        private readonly AppDbContext _context;

        public ClubesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Clubes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Traemos los clubes
            var clubes = await _context.Clubes.ToListAsync();

            // 2. Traemos los nombres de las asociaciones en un diccionario para la vista
            var dictAsociaciones = await _context.AsociacionesProvinciales
                .ToDictionaryAsync(a => a.IdAsociacion, a => a.NombreAsociacion);

            ViewBag.NombresAsociaciones = dictAsociaciones;

            return View(clubes);
        }

        // GET: /Clubes/Create (Este muestra la pantalla vacía)
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales.Where(a => a.EstadoActivo == true).ToList(), "IdAsociacion", "NombreAsociacion");
            return View();
        }

        // POST: /Clubes/Create (Este recibe los datos y los guarda en BD)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Club club)
        {
            // 1. Ignorar las propiedades de relación para que la validación pase
            ModelState.Remove("CompetenciasParticipadas");
            ModelState.Remove("Asociacion");

            if (ModelState.IsValid)
            {
                _context.Add(club);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Club registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Si hay error en la validación, recarga el Select
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales.Where(a => a.EstadoActivo == true).ToList(), "IdAsociacion", "NombreAsociacion");
            return View(club);
        }
        // GET: /Clubes/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var club = await _context.Clubes.FindAsync(id);
            if (club == null) return NotFound();

            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales, "IdAsociacion", "NombreAsociacion", club.IdAsociacion);
            return View(club);
        }

        // POST: /Clubes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Club club)
        {
            if (id != club.IdClub) return NotFound();

            // 1. Ignorar las propiedades de relación
            ModelState.Remove("CompetenciasParticipadas");
            ModelState.Remove("Asociacion");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Club actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clubes.Any(e => e.IdClub == club.IdClub))
                        return NotFound();
                    else
                        throw;
                }
            }
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales, "IdAsociacion", "NombreAsociacion", club.IdAsociacion);
            return View(club);
        }
        // POST: /Clubes/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _context.Clubes.FindAsync(id);
            if (club != null)
            {
                // Validación para evitar borrar un club que ya tiene atletas
                bool tieneAtletas = await _context.Atletas.AnyAsync(a => a.IdClub == id);
                if (tieneAtletas)
                {
                    TempData["Error"] = "No se puede eliminar el club porque tiene atletas registrados.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Clubes.Remove(club);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Club eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }// GET: /Clubes/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // 1. Buscamos el club
            var club = await _context.Clubes.FindAsync(id);
            if (club == null) return NotFound();

            // 2. Buscamos el nombre de la Asociación para mostrarlo en la vista
            var asociacion = await _context.AsociacionesProvinciales.FindAsync(club.IdAsociacion);
            ViewBag.NombreAsociacion = asociacion != null ? asociacion.NombreAsociacion : "Sin Asociación";

            // 3. Buscamos TODOS los atletas que pertenezcan a este club específico
            var atletasDelClub = await _context.Atletas
                                               .Where(a => a.IdClub == id)
                                               .OrderBy(a => a.Apellidos)
                                               .ToListAsync();

            // Pasamos la lista de atletas a la vista usando ViewBag
            ViewBag.Atletas = atletasDelClub;

            return View(club);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var club = await _context.Clubes.FindAsync(id);
            if (club == null) return NotFound();

            club.EstadoActivo = !club.EstadoActivo;

            // Si el club se INACTIVA, inactivamos a sus atletas
            if (!club.EstadoActivo)
            {
                var atletas = _context.Atletas.Where(a => a.IdClub == id).ToList();
                foreach (var atleta in atletas)
                {
                    atleta.EstadoActivo = false;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = club.EstadoActivo ? "Club habilitado." : "Club y sus atletas inhabilitados.";

            return RedirectToAction(nameof(Index));
        }

    }
}