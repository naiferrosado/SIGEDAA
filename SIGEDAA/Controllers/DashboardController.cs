using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;

namespace SIGEDAA.Controllers;

[Authorize(Roles = "Administrador,Presidente,Juez")]
public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.AtletasCount = await _context.Atletas.CountAsync();
        ViewBag.AtletasActivosCount = await _context.Atletas.CountAsync(a => a.EstadoActivo);
        ViewBag.ClubesCount = await _context.Clubes.CountAsync();
        ViewBag.ClubesActivosCount = await _context.Clubes.CountAsync(c => c.EstadoActivo);
        ViewBag.TorneosCount = await _context.Competencias.CountAsync();
        ViewBag.TorneosActivosCount = await _context.Competencias.CountAsync(c => c.Estado != "Finalizada");
        ViewBag.ResultadosCount = await _context.ResultadosCompetencia.CountAsync();
        ViewBag.AsociacionesActivasCount = await _context.AsociacionesProvinciales.CountAsync(a => a.EstadoActivo);
        ViewBag.DisciplinasCount = await _context.Disciplinas.CountAsync();
        ViewBag.UsuariosActivosCount = await _context.Usuarios.CountAsync(u => u.EstadoActivo);

        return View();
    }
}
