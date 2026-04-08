using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Text;

namespace SIGEDAA.Controllers;

[Authorize(Roles = "Administrador,Juez")]
public class CompetenciasController : Controller
{
    private readonly AppDbContext _context;

    public CompetenciasController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        Competencia[] competencias = await _context.Competencias.ToArrayAsync();
        bool huboCambios = false;

        foreach (Competencia comp in competencias)
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
        if (id is null)
        {
            return NotFound();
        }

        Competencia? competencia = await _context.Competencias
            .Include(c => c.ClubesInscritos)
            .ThenInclude(cc => cc.Club)
            .FirstOrDefaultAsync(m => m.IdCompetencia == id);

        if (competencia is null)
        {
            return NotFound();
        }

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
            ModelState.AddModelError("FechaFin", "La fecha de finalizacion no puede ser antes del inicio.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(competencia);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Competencia registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        return View(competencia);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Competencia? competencia = await _context.Competencias.FindAsync(id);
        if (competencia is null)
        {
            return NotFound();
        }

        return View(competencia);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Competencia competencia)
    {
        if (id != competencia.IdCompetencia)
        {
            return NotFound();
        }

        ModelState.Remove("ClubesInscritos");

        if (competencia.FechaFin.Date < competencia.FechaInicio.Date)
        {
            ModelState.AddModelError("FechaFin", "La fecha de finalizacion no puede ser antes del inicio.");
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
                {
                    return NotFound();
                }

                throw;
            }
        }

        return View(competencia);
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerClubesPorProvincia(int idAsociacion)
    {
        object[] clubes = await _context.Clubes
            .Where(c => c.IdAsociacion == idAsociacion && c.EstadoActivo)
            .Select(c => new
            {
                valor = c.IdClub,
                texto = c.NombreClub
            })
            .ToArrayAsync();

        return Json(clubes);
    }

    [HttpGet]
    public async Task<IActionResult> InscribirClub(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Competencia? competencia = await _context.Competencias.FindAsync(id);
        if (competencia is null)
        {
            return NotFound();
        }

        ViewBag.Competencia = competencia;
        AsociacionProvincial[] asociaciones = await _context.AsociacionesProvinciales.ToArrayAsync();
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
            TempData["Error"] = "Este club ya se encuentra inscrito en esta competencia.";
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

        TempData["Success"] = "Club inscrito correctamente en la competencia.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Competencia? competencia = await _context.Competencias.FindAsync(id);
        if (competencia is not null)
        {
            _context.Competencias.Remove(competencia);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Competencia eliminada correctamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<FileResult> ExportarCsv()
    {
        Competencia[] competencias = await _context.Competencias
            .OrderBy(c => c.FechaInicio)
            .ToArrayAsync();

        var csv = new StringBuilder();
        csv.AppendLine("NombreEvento,Sede,FechaInicio,FechaFin,Nivel,Estado");

        foreach (Competencia competencia in competencias)
        {
            csv.AppendLine(string.Join(",",
                EscaparCsv(competencia.NombreEvento),
                EscaparCsv(competencia.Sede),
                competencia.FechaInicio.ToString("yyyy-MM-dd"),
                competencia.FechaFin.ToString("yyyy-MM-dd"),
                EscaparCsv(competencia.Nivel),
                EscaparCsv(competencia.Estado)));
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"competencias-{DateTime.Now:yyyyMMdd}.csv");
    }

    private static string EscaparCsv(string? valor)
    {
        string limpio = valor ?? string.Empty;
        return $"\"{limpio.Replace("\"", "\"\"")}\"";
    }
}
