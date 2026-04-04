using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class AsociacionesController : Controller
    {
        private readonly AppDbContext _context;

        public AsociacionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Asociaciones
        public async Task<IActionResult> Index()
        {
            var asociaciones = await _context.AsociacionesProvinciales.ToListAsync();
            return View(asociaciones);
        }

        // GET: /Asociaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Asociaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AsociacionProvincial asociacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asociacion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Asociación registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(asociacion);
        }

        // GET: /Asociaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var asociacion = await _context.AsociacionesProvinciales.FindAsync(id);
            if (asociacion == null) return NotFound();

            return View(asociacion);
        }

        // POST: /Asociaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AsociacionProvincial asociacion)
        {
            if (id != asociacion.IdAsociacion) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asociacion);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Asociación actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AsociacionesProvinciales.Any(e => e.IdAsociacion == asociacion.IdAsociacion))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(asociacion);
        }

        // POST: /Asociaciones/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asociacion = await _context.AsociacionesProvinciales.FindAsync(id);
            if (asociacion != null)
            {
                _context.AsociacionesProvinciales.Remove(asociacion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Asociación eliminada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var asociacion = await _context.AsociacionesProvinciales.FindAsync(id);
            if (asociacion == null) return NotFound();

            // Cambiamos el estado de la asociación
            asociacion.EstadoActivo = !asociacion.EstadoActivo;

            // Si la asociación se acaba de INACTIVAR, aplicamos la cascada
            if (!asociacion.EstadoActivo)
            {
                // 1. Buscamos todos los clubes de esta asociación
                var clubes = _context.Clubes.Where(c => c.IdAsociacion == id).ToList();

                foreach (var club in clubes)
                {
                    club.EstadoActivo = false; // Inactivar Club

                    // 2. Buscamos todos los atletas de este club y los inactivamos
                    var atletas = _context.Atletas.Where(a => a.IdClub == club.IdClub).ToList();
                    foreach (var atleta in atletas)
                    {
                        atleta.EstadoActivo = false; // Inactivar Atleta
                    }
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = asociacion.EstadoActivo ? "Asociación habilitada." : "Asociación y toda su estructura inhabilitada.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var asociacion = await _context.AsociacionesProvinciales.FindAsync(id);
            if (asociacion == null) return NotFound();

            // Buscamos los clubes de esta asociación
            var clubes = await _context.Clubes
                .Where(c => c.IdAsociacion == id)
                .OrderBy(c => c.NombreClub)
                .ToListAsync();

            ViewBag.Clubes = clubes;
            return View(asociacion);
        }
    }
}