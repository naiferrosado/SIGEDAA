using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEDAA.Data;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Models;
using System.Linq;

[Authorize]
public class ResultadosController : Controller
{
    private readonly AppDbContext _context;
    public ResultadosController(AppDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var resultados = await _context.ResultadosCompetencia
            .Include(r => r.Competencia).Include(r => r.Atleta).Include(r => r.Disciplina)
            .OrderByDescending(r => r.IdResultado).ToListAsync();
        return View(resultados);
    }

    // GET: /Resultados/Create
    public IActionResult Create()
    {
        ViewBag.Competencias = new SelectList(_context.Competencias.Where(c => c.Estado != "Cancelada"), "IdCompetencia", "NombreEvento");

        // ESTA ES LA CLAVE: Lo mandamos vacío desde el inicio.
        ViewBag.Atletas = new SelectList(new List<SelectListItem>(), "Value", "Text");

        ViewBag.Disciplinas = new SelectList(_context.Disciplinas, "IdDisciplina", "NombreDisciplina");

        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResultadoCompetencia resultado)
    {
        // Limpiamos TODAS las validaciones de objetos que no vienen del formulario
        ModelState.Remove("Competencia");
        ModelState.Remove("Atleta");
        ModelState.Remove("Club");
        ModelState.Remove("Disciplina");

        if (ModelState.IsValid)
        {
            _context.Add(resultado);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Resultado guardado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // SI LLEGA AQUÍ ES PORQUE FALLÓ. Vamos a recargar todo:
        ViewBag.Competencias = new SelectList(_context.Competencias, "IdCompetencia", "NombreEvento", resultado.IdCompetencia);
        ViewBag.Disciplinas = new SelectList(_context.Disciplinas, "IdDisciplina", "NombreDisciplina", resultado.IdDisciplina);

        return View(resultado);
    }

    // Modificamos este método para que también nos devuelva el CLUB del atleta
    [HttpGet]
    public async Task<JsonResult> ObtenerAtletasPorTorneo(int idCompetencia)
    {
        var clubesIds = await _context.CompetenciasClubes
            .Where(cc => cc.IdCompetencia == idCompetencia)
            .Select(cc => cc.IdClub).ToListAsync();

        var atletas = await _context.Atletas
            .Where(a => clubesIds.Contains(a.IdClub) && a.EstadoActivo)
            .Select(a => new {
                valor = a.IdAtleta,
                texto = a.Nombres + " " + a.Apellidos,
                clubId = a.IdClub,           // <--- MANDAMOS EL ID DEL CLUB
                clubNombre = a.Club.NombreClub // <--- MANDAMOS EL NOMBRE
            }).ToListAsync();

        return Json(atletas);
    }

}