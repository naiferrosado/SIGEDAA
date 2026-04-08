using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class CompetenciasController : Controller
    {
        // SIN private readonly
        public AppDbContext _context;

        public CompetenciasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Cambiado a ToArrayAsync()
            IEnumerable<Competencia> competencias = await _context.Competencias.ToArrayAsync();
            bool huboCambios = false;

            foreach (var comp in competencias)
            {
                if (comp.FechaFin.Date < DateTime.Now.Date && comp.Estado != "Finalizada")
                {
                    comp.Estado = "Finalizada";
                    _context.Update(comp);
                    huboCambios = true;
                }
            }

            if (huboCambios)
            {
                await _context.SaveChangesAsync();
            }

            return View(competencias);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var competencia = await _context.Competencias
                .Include(c => c.ClubesInscritos)
                    .ThenInclude(cc => cc.Club)
                .FirstOrDefaultAsync(m => m.IdCompetencia == id);

            if (competencia == null) return NotFound();

            if (competencia.FechaFin.Date < DateTime.Now.Date && competencia.Estado != "Finalizada")
            {
                competencia.Estado = "Finalizada";
                _context.Update(competencia);
                await _context.SaveChangesAsync();
            }

            return View(competencia);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Competencia competencia)
        {
            ModelState.Remove("ClubesInscritos");
            if (competencia.FechaFin.Date < competencia.FechaInicio.Date)
            {
                ModelState.AddModelError("FechaFin", "La fecha de finalización no puede ser antes del inicio.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(competencia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Torneo/Competencia registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(competencia);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var competencia = await _context.Competencias.FindAsync(id);
            if (competencia == null) return NotFound();

            return View(competencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Competencia competencia)
        {
            if (id != competencia.IdCompetencia) return NotFound();
            ModelState.Remove("ClubesInscritos");

            if (competencia.FechaFin.Date < competencia.FechaInicio.Date)
            {
                ModelState.AddModelError("FechaFin", "La fecha de finalización no puede ser antes del inicio.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competencia);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Competencia actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Competencias.Any(e => e.IdCompetencia == competencia.IdCompetencia))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(competencia);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerClubesPorProvincia(int idAsociacion)
        {
            // Cambiado a ToArrayAsync()
            IEnumerable<object> clubes = await _context.Clubes
                .Where(c => c.IdAsociacion == idAsociacion && c.EstadoActivo == true)
                .Select(c => new {
                    valor = c.IdClub,
                    texto = c.NombreClub
                })
                .ToArrayAsync();

            return Json(clubes);
        }

        [HttpGet]
        public async Task<IActionResult> InscribirClub(int? id)
        {
            if (id == null) return NotFound();

            var competencia = await _context.Competencias.FindAsync(id);
            if (competencia == null) return NotFound();

            ViewBag.Competencia = competencia;
            // Evitamos la lista aquí también
            IEnumerable<AsociacionProvincial> asociaciones = await _context.AsociacionesProvinciales.ToArrayAsync();
            ViewBag.Asociaciones = new SelectList(asociaciones, "IdAsociacion", "NombreAsociacion");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InscribirClub(int IdCompetencia, int IdClub)
        {
            bool yaInscrito = await _context.CompetenciasClubes
                .AnyAsync(cc => cc.IdCompetencia == IdCompetencia && cc.IdClub == IdClub);

            if (yaInscrito)
            {
                TempData["Error"] = "Este club ya se encuentra inscrito en este torneo.";
                return RedirectToAction(nameof(InscribirClub), new { id = IdCompetencia });
            }

            var nuevaInscripcion = new CompetenciaClub
            {
                IdCompetencia = IdCompetencia,
                IdClub = IdClub,
                EstadoInscripcion = "Confirmado",
                FechaInscripcion = DateTime.Now
            };

            _context.Add(nuevaInscripcion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Club inscrito exitosamente en el torneo.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competencia = await _context.Competencias.FindAsync(id);
            if (competencia != null)
            {
                _context.Competencias.Remove(competencia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Competencia eliminada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}