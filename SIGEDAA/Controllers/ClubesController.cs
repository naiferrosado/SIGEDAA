using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize(Roles = "Administrador,Presidente,Juez")]
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
        // GET: /Clubes/Create
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpGet]
        public IActionResult Create()
        {
            // 1. Cargamos las Asociaciones
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales.Where(a => a.EstadoActivo == true).ToList(), "IdAsociacion", "NombreAsociacion");

            // 2. CARGAMOS LOS ENTRENADORES (Asegúrate de que este código esté aquí)
            var listaEntrenadores = _context.Entrenadores.Where(e => e.EstadoActivo)
                .Select(e => new { Id = e.IdEntrenador, NombreCompleto = e.Nombres + " " + e.Apellidos }).ToList();

            ViewBag.Entrenadores = new SelectList(listaEntrenadores, "Id", "NombreCompleto");

            return View();
        }
        // POST: /Clubes/Create (Este recibe los datos y los guarda en BD)
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Club club)
        {
            // 1. Ignorar las propiedades de relación para que la validación pase
            ModelState.Remove("CompetenciasParticipadas");
            ModelState.Remove("Asociacion");
            ModelState.Remove("CompetenciasParticipadas");
            ModelState.Remove("Asociacion");
            ModelState.Remove("EntrenadorPrincipal");
            if (ModelState.IsValid)
            {
                _context.Add(club);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Club registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Si hay error en la validación, recarga el Select
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales.Where(a => a.EstadoActivo == true).ToList(), "IdAsociacion", "NombreAsociacion");
            var listaEntrenadores = _context.Entrenadores.Where(e => e.EstadoActivo)
                 .Select(e => new { Id = e.IdEntrenador, NombreCompleto = e.Nombres + " " + e.Apellidos }).ToList();

            ViewBag.Entrenadores = new SelectList(listaEntrenadores, "Id", "NombreCompleto");
            return View(club);
        }
        // GET: /Clubes/Edit/5
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var club = await _context.Clubes.FindAsync(id);
            if (club == null) return NotFound();

            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales, "IdAsociacion", "NombreAsociacion", club.IdAsociacion);
            var listaEntrenadores = _context.Entrenadores.Where(e => e.EstadoActivo)
                 .Select(e => new { Id = e.IdEntrenador, NombreCompleto = e.Nombres + " " + e.Apellidos }).ToList();

            ViewBag.Entrenadores = new SelectList(listaEntrenadores, "Id", "NombreCompleto");
            return View(club);
        }

        // POST: /Clubes/Edit/5
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Club club)
        {
            if (id != club.IdClub) return NotFound();

            // 1. Ignorar las propiedades de relación
            ModelState.Remove("CompetenciasParticipadas");
            ModelState.Remove("Asociacion");
            ModelState.Remove("CompetenciasParticipadas");
            ModelState.Remove("Asociacion");
            ModelState.Remove("EntrenadorPrincipal");
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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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

        [HttpGet]
        public async Task<FileResult> ExportarCsv()
        {
            List<Club> clubes = await _context.Clubes.OrderBy(c => c.NombreClub).ToListAsync();
            Dictionary<int, string> asociaciones = await _context.AsociacionesProvinciales
                .ToDictionaryAsync(a => a.IdAsociacion, a => a.NombreAsociacion);

            var csv = new StringBuilder();
            csv.AppendLine("NombreClub,Asociacion,Telefono,FechaInscripcion,Estado");

            foreach (Club club in clubes)
            {
                asociaciones.TryGetValue(club.IdAsociacion, out string? nombreAsociacion);
                csv.AppendLine(string.Join(",",
                    EscaparCsv(club.NombreClub),
                    EscaparCsv(nombreAsociacion),
                    EscaparCsv(club.Telefono),
                    club.FechaInscripcion.ToString("yyyy-MM-dd"),
                    club.EstadoActivo ? "Activo" : "Inactivo"));
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"clubes-{DateTime.Now:yyyyMMdd}.csv");
        }

        private static string EscaparCsv(string? valor)
        {
            string limpio = valor ?? string.Empty;
            return $"\"{limpio.Replace("\"", "\"\"")}\"";
        }

    }
}
