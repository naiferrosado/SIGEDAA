using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class CompetenciasController : Controller
    {
        private readonly AppDbContext _context;

        public CompetenciasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Competencias
        public async Task<IActionResult> Index()
        {
            var competencias = await _context.Competencias.ToListAsync();
            bool huboCambios = false;

            // MAGIA AUTOMÁTICA: Revisamos si algún torneo ya caducó
            foreach (var comp in competencias)
            {
                // Si la fecha de finalización ya pasó y no está finalizado...
                if (comp.FechaFin.Date < DateTime.Now.Date && comp.Estado != "Finalizada")
                {
                    comp.Estado = "Finalizada"; // Lo cerramos automáticamente
                    _context.Update(comp);
                    huboCambios = true;
                }
            }

            if (huboCambios)
            {
                await _context.SaveChangesAsync(); // Guardamos los cambios silenciosamente
            }

            return View(competencias);
        }

        // GET: /Competencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Buscamos el torneo e INCLUIMOS la tabla puente y los clubes
            var competencia = await _context.Competencias
                .Include(c => c.ClubesInscritos)       // Trae las inscripciones
                    .ThenInclude(cc => cc.Club)        // Trae los datos del Club de cada inscripción
                .FirstOrDefaultAsync(m => m.IdCompetencia == id);

            if (competencia == null) return NotFound();

            // Verificación automática por si entran directo por el enlace
            if (competencia.FechaFin.Date < DateTime.Now.Date && competencia.Estado != "Finalizada")
            {
                competencia.Estado = "Finalizada";
                _context.Update(competencia);
                await _context.SaveChangesAsync();
            }

            return View(competencia);
        }

        // GET: /Competencias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Competencias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Competencia competencia)
        {
            ModelState.Remove("ClubesInscritos");
            // NUEVA VALIDACIÓN: Bloquear fechas ilógicas
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

        // GET: /Competencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var competencia = await _context.Competencias.FindAsync(id);
            if (competencia == null) return NotFound();

            return View(competencia);
        }

        // POST: /Competencias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Competencia competencia)
        {
            if (id != competencia.IdCompetencia) return NotFound();
            ModelState.Remove("ClubesInscritos");
            // NUEVA VALIDACIÓN: Bloquear fechas ilógicas
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
        // 1. MÉTODO PARA EL AJAX (Filtro en cascada)
        [HttpGet]
        public async Task<JsonResult> ObtenerClubesPorProvincia(int idAsociacion)
        {
            // Busca solo los clubes activos de esa provincia
            var clubes = await _context.Clubes
                .Where(c => c.IdAsociacion == idAsociacion && c.EstadoActivo == true)
                .Select(c => new {
                    valor = c.IdClub,
                    texto = c.NombreClub
                })
                .ToListAsync();

            return Json(clubes);
        }

        // 2. GET: Muestra la pantalla de inscripción
        [HttpGet]
        public async Task<IActionResult> InscribirClub(int? id)
        {
            if (id == null) return NotFound();

            var competencia = await _context.Competencias.FindAsync(id);
            if (competencia == null) return NotFound();

            ViewBag.Competencia = competencia;
            // Mandamos la lista de provincias para el primer dropdown
            ViewBag.Asociaciones = new SelectList(await _context.AsociacionesProvinciales.ToListAsync(), "IdAsociacion", "NombreAsociacion");

            return View();
        }

        // 3. POST: Guarda la inscripción en la base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InscribirClub(int IdCompetencia, int IdClub)
        {
            // Verificamos si este club ya está inscrito en este torneo para evitar duplicados
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
        // POST: /Competencias/Delete
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