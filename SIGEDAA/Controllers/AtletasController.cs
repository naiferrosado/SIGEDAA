using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize(Roles = "Administrador,Presidente,Juez")]
    public class AtletasController : Controller
    {
        private readonly AppDbContext _context;

        public AtletasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Atletas
        public async Task<IActionResult> Index()
        {
            var atletas = await _context.Atletas.OrderBy(a => a.IdAtleta).ToListAsync();
            return View(atletas);
        }

        // GET: /Atletas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var atleta = await _context.Atletas.FirstOrDefaultAsync(a => a.IdAtleta == id);
            if (atleta == null) return NotFound();

            return View(atleta);
        }

        // GET: /Atletas/Create
        [Authorize(Roles = "Administrador,Presidente")]
        public IActionResult Create()
        {
            // Enviamos la lista de Asociaciones para el primer filtro
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales.ToList(), "IdAsociacion", "NombreAsociacion");

            // El de clubes lo mandamos vacío al inicio
            ViewBag.Clubes = new SelectList(new List<Club>(), "IdClub", "NombreClub");

            return View();
        }

        // POST: /Atletas/Create
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Atleta atleta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(atleta);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Atleta agregado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Si hay error en el formulario, recargamos las listas
            ViewBag.Asociaciones = new SelectList(_context.AsociacionesProvinciales.ToList(), "IdAsociacion", "NombreAsociacion");
            ViewBag.Clubes = new SelectList(_context.Clubes.Where(c => c.IdClub == atleta.IdClub).ToList(), "IdClub", "NombreClub", atleta.IdClub);

            return View(atleta);
        }

        // ESTE ES EL NUEVO MÉTODO PARA EL FILTRO (Ponlo al final del controlador, antes de la última llave)
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpGet]
        public async Task<JsonResult> ObtenerClubesPorAsociacion(int idAsociacion)
        {
            var clubes = await _context.Clubes
                .Where(c => c.IdAsociacion == idAsociacion && c.EstadoActivo == true)
                .Select(c => new {
                    valor = c.IdClub,
                    texto = c.NombreClub
                })
                .ToListAsync();

            return Json(clubes);
        }

        // GET: /Atletas/Edit/5
        [Authorize(Roles = "Administrador,Presidente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta == null) return NotFound();

            ViewBag.Clubes = new SelectList(_context.Clubes.ToList(), "IdClub", "NombreClub", atleta.IdClub);
            return View(atleta);
        }

        // POST: /Atletas/Edit/5
        [Authorize(Roles = "Administrador,Presidente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Atleta atleta)
        {
            if (id != atleta.IdAtleta) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atleta);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Atleta editado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Atletas.Any(e => e.IdAtleta == atleta.IdAtleta))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.Clubes = new SelectList(_context.Clubes.ToList(), "IdClub", "NombreClub", atleta.IdClub);
            return View(atleta);
        }

        // GET: /Atletas/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var atleta = await _context.Atletas.FirstOrDefaultAsync(a => a.IdAtleta == id);
            if (atleta == null) return NotFound();

            return View(atleta);
        }

        // POST: /Atletas/Delete
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta != null)
            {
                _context.Atletas.Remove(atleta);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Atleta eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta != null)
            {
                atleta.EstadoActivo = !atleta.EstadoActivo;
                await _context.SaveChangesAsync();
                TempData["Success"] = atleta.EstadoActivo ? "Atleta habilitado." : "Atleta inhabilitado.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<FileResult> ExportarCsv()
        {
            Atleta[] atletas = await _context.Atletas
                .OrderBy(a => a.Apellidos)
                .ThenBy(a => a.Nombres)
                .ToArrayAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Id,Nombres,Apellidos,FechaNacimiento,Genero,EstaturaCm,PesoKg,TipoSangre,Estado");

            foreach (Atleta atleta in atletas)
            {
                csv.AppendLine(string.Join(",",
                    atleta.IdAtleta,
                    EscaparCsv(atleta.Nombres),
                    EscaparCsv(atleta.Apellidos),
                    atleta.FechaNacimiento.ToString("yyyy-MM-dd"),
                    EscaparCsv(atleta.Genero),
                    atleta.EstaturaCm,
                    atleta.PesoKg,
                    EscaparCsv(atleta.TipoSangre),
                    atleta.EstadoActivo ? "Activo" : "Inactivo"));
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"atletas-{DateTime.Now:yyyyMMdd}.csv");
        }

        private static string EscaparCsv(string? valor)
        {
            string limpio = valor ?? string.Empty;
            return $"\"{limpio.Replace("\"", "\"\"")}\"";
        }
    }
}
