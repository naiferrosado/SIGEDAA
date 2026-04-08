using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Text;

namespace SIGEDAA.Controllers;

[Authorize(Roles = "Administrador,Juez")]
public class ResultadosController : Controller
{
    private readonly AppDbContext _context;

    public ResultadosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ResultadoCompetencia[] resultados = await _context.ResultadosCompetencia
            .Include(r => r.Competencia)
            .Include(r => r.Atleta)
            .Include(r => r.Disciplina)
            .Include(r => r.Club)
            .OrderByDescending(r => r.IdCompetencia)
            .ThenBy(r => r.IdDisciplina)
            .ThenBy(r => r.PosicionFinal)
            .ToArrayAsync();

        return View(resultados);
    }

    [HttpGet]
    public IActionResult Create()
    {
        Competencia[] competenciasActivas = _context.Competencias
            .Where(c => c.Estado != "Cancelada")
            .ToArray();

        ViewBag.Competencias = new SelectList(competenciasActivas, "IdCompetencia", "NombreEvento");
        ViewBag.Atletas = new SelectList(Array.Empty<SelectListItem>(), "Value", "Text");
        ViewBag.Disciplinas = new SelectList(Array.Empty<SelectListItem>(), "Value", "Text");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResultadoCompetencia resultado)
    {
        ModelState.Remove("Competencia");
        ModelState.Remove("Atleta");
        ModelState.Remove("Club");
        ModelState.Remove("Disciplina");

        if (ModelState.IsValid)
        {
            resultado.PosicionFinal = 0;
            _context.Add(resultado);
            await _context.SaveChangesAsync();

            Disciplina? disciplina = await _context.Disciplinas.FindAsync(resultado.IdDisciplina);

            ResultadoCompetencia[] competidores = await _context.ResultadosCompetencia
                .Where(r => r.IdCompetencia == resultado.IdCompetencia && r.IdDisciplina == resultado.IdDisciplina)
                .ToArrayAsync();

            if (disciplina is not null && disciplina.TipoMedicion == "Distancia")
            {
                competidores = competidores
                    .OrderByDescending(r => decimal.TryParse(r.MarcaObtenida, out decimal marca) ? marca : 0)
                    .ToArray();
            }
            else
            {
                competidores = competidores
                    .OrderBy(r => decimal.TryParse(r.MarcaObtenida, out decimal marca) ? marca : decimal.MaxValue)
                    .ToArray();
            }

            for (int i = 0; i < competidores.Length; i++)
            {
                competidores[i].PosicionFinal = i + 1;
                _context.Update(competidores[i]);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Marca registrada y posiciones recalculadas.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Competencias = new SelectList(_context.Competencias, "IdCompetencia", "NombreEvento", resultado.IdCompetencia);
        return View(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        ResultadoCompetencia? resultado = await _context.ResultadosCompetencia
            .Include(r => r.Competencia)
            .Include(r => r.Atleta)
            .Include(r => r.Disciplina)
            .Include(r => r.Club)
            .FirstOrDefaultAsync(m => m.IdResultado == id);

        if (resultado is null)
        {
            return NotFound();
        }

        return View(resultado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ResultadoCompetencia modelo)
    {
        if (id != modelo.IdResultado)
        {
            return NotFound();
        }

        ModelState.Remove("Competencia");
        ModelState.Remove("Atleta");
        ModelState.Remove("Club");
        ModelState.Remove("Disciplina");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(modelo);
                await _context.SaveChangesAsync();

                Disciplina? disciplina = await _context.Disciplinas.FindAsync(modelo.IdDisciplina);

                ResultadoCompetencia[] competidores = await _context.ResultadosCompetencia
                    .Where(r => r.IdCompetencia == modelo.IdCompetencia && r.IdDisciplina == modelo.IdDisciplina)
                    .ToArrayAsync();

                if (disciplina is not null && disciplina.TipoMedicion == "Distancia")
                {
                    competidores = competidores
                        .OrderByDescending(r => decimal.TryParse(r.MarcaObtenida, out decimal marca) ? marca : 0)
                        .ToArray();
                }
                else
                {
                    competidores = competidores
                        .OrderBy(r => decimal.TryParse(r.MarcaObtenida, out decimal marca) ? marca : decimal.MaxValue)
                        .ToArray();
                }

                for (int i = 0; i < competidores.Length; i++)
                {
                    competidores[i].PosicionFinal = i + 1;
                    _context.Update(competidores[i]);
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Marca actualizada y posiciones recalculadas.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar: " + ex.Message);
            }
        }

        modelo.Competencia = await _context.Competencias.FindAsync(modelo.IdCompetencia);
        modelo.Atleta = await _context.Atletas.FindAsync(modelo.IdAtleta);
        modelo.Disciplina = await _context.Disciplinas.FindAsync(modelo.IdDisciplina);
        modelo.Club = await _context.Clubes.FindAsync(modelo.IdClub);

        return View(modelo);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        ResultadoCompetencia? resultado = await _context.ResultadosCompetencia.FindAsync(id);
        if (resultado is not null)
        {
            int idComp = resultado.IdCompetencia;
            int idDisc = resultado.IdDisciplina;

            _context.ResultadosCompetencia.Remove(resultado);
            await _context.SaveChangesAsync();

            ResultadoCompetencia[] competidores = await _context.ResultadosCompetencia
                .Where(r => r.IdCompetencia == idComp && r.IdDisciplina == idDisc)
                .OrderBy(r => r.PosicionFinal)
                .ToArrayAsync();

            for (int i = 0; i < competidores.Length; i++)
            {
                competidores[i].PosicionFinal = i + 1;
                _context.Update(competidores[i]);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Marca eliminada correctamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerAtletasPorTorneo(int idCompetencia)
    {
        int[] clubesIds = await _context.CompetenciasClubes
            .Where(cc => cc.IdCompetencia == idCompetencia)
            .Select(cc => cc.IdClub)
            .ToArrayAsync();

        object[] atletas = await _context.Atletas
            .Where(a => clubesIds.Contains(a.IdClub) && a.EstadoActivo)
            .Select(a => new
            {
                valor = a.IdAtleta,
                texto = a.Nombres + " " + a.Apellidos,
                clubId = a.IdClub,
                clubNombre = a.Club != null ? a.Club.NombreClub : string.Empty
            })
            .ToArrayAsync();

        return Json(atletas);
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerDisciplinasPorTorneo(int idCompetencia)
    {
        object[] disciplinas = await _context.Disciplinas
            .Select(d => new
            {
                valor = d.IdDisciplina,
                texto = d.NombreDisciplina
            })
            .ToArrayAsync();

        return Json(disciplinas);
    }

    [HttpGet]
    public async Task<FileResult> ExportarCsv()
    {
        ResultadoCompetencia[] resultados = await _context.ResultadosCompetencia
            .Include(r => r.Competencia)
            .Include(r => r.Disciplina)
            .Include(r => r.Atleta)
            .Include(r => r.Club)
            .OrderBy(r => r.IdCompetencia)
            .ThenBy(r => r.IdDisciplina)
            .ThenBy(r => r.PosicionFinal)
            .ToArrayAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Competencia,Disciplina,Atleta,Club,Marca,Posicion");

        foreach (ResultadoCompetencia resultado in resultados)
        {
            csv.AppendLine(string.Join(",",
                EscaparCsv(resultado.Competencia?.NombreEvento),
                EscaparCsv(resultado.Disciplina?.NombreDisciplina),
                EscaparCsv($"{resultado.Atleta?.Nombres} {resultado.Atleta?.Apellidos}".Trim()),
                EscaparCsv(resultado.Club?.NombreClub),
                EscaparCsv(resultado.MarcaObtenida),
                resultado.PosicionFinal));
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"resultados-{DateTime.Now:yyyyMMdd}.csv");
    }

    private static string EscaparCsv(string? valor)
    {
        string limpio = valor ?? string.Empty;
        return $"\"{limpio.Replace("\"", "\"\"")}\"";
    }
}
