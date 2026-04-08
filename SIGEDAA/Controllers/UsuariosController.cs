using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using SIGEDAA.Services;

namespace SIGEDAA.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IAuditTrailService _auditTrailService;

    public UsuariosController(
        AppDbContext context,
        IPasswordService passwordService,
        IAuditTrailService auditTrailService)
    {
        _context = context;
        _passwordService = passwordService;
        _auditTrailService = auditTrailService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Usuarios.OrderBy(u => u.NombreCompleto).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.IdUsuario == id);
        if (usuario is null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    public IActionResult Create()
    {
        return View(new Usuario { EstadoActivo = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            try
            {
                bool existeCorreo = await _context.Usuarios.AnyAsync(u => u.CorreoElectronico == usuario.CorreoElectronico);
                if (existeCorreo)
                {
                    ModelState.AddModelError("CorreoElectronico", "Este correo ya esta registrado en el sistema.");
                    return View(usuario);
                }

                usuario.FechaRegistro = DateTime.Now;
                usuario.ClaveAcceso = _passwordService.HashPassword(usuario, usuario.ClaveAcceso);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                await _auditTrailService.RecordAsync(
                    "UsuarioCreado",
                    $"Se creo el usuario {usuario.CorreoElectronico} con rol {usuario.Rol}.",
                    User.Identity?.Name);

                TempData["Success"] = "Usuario registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al guardar: " + ex.Message);
            }
        }

        return View(usuario);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Usuario? usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null)
        {
            return NotFound();
        }

        usuario.ClaveAcceso = string.Empty;
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Usuario usuario)
    {
        if (id != usuario.IdUsuario)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Usuario.ClaveAcceso));

        if (ModelState.IsValid)
        {
            try
            {
                Usuario? usuarioOriginal = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);
                if (usuarioOriginal is null)
                {
                    return NotFound();
                }

                bool existeCorreo = await _context.Usuarios.AnyAsync(u =>
                    u.CorreoElectronico == usuario.CorreoElectronico &&
                    u.IdUsuario != usuario.IdUsuario);

                if (existeCorreo)
                {
                    ModelState.AddModelError("CorreoElectronico", "Este correo ya esta registrado en el sistema.");
                    return View(usuario);
                }

                usuario.FechaRegistro = usuarioOriginal.FechaRegistro;
                usuario.ClaveAcceso = string.IsNullOrWhiteSpace(usuario.ClaveAcceso)
                    ? usuarioOriginal.ClaveAcceso
                    : _passwordService.HashPassword(usuario, usuario.ClaveAcceso);

                _context.Update(usuario);
                await _context.SaveChangesAsync();
                await _auditTrailService.RecordAsync(
                    "UsuarioActualizado",
                    $"Se actualizo el usuario {usuario.CorreoElectronico}. Estado activo: {usuario.EstadoActivo}.",
                    User.Identity?.Name);

                TempData["Success"] = "Usuario actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar: " + ex.Message);
            }
        }

        return View(usuario);
    }
}
