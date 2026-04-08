using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    // Solo los usuarios con rol "Administrador" pueden entrar a este módulo
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }


        // INDEX (Lista de Usuarios)

        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }


        // DETAILS (Ver Perfil de Usuario)

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null) return NotFound();

            return View(usuario);
        }


        // CREATE (Crear Nuevo Usuario)

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Validar que el correo no exista ya en la base de datos
                    var existeCorreo = await _context.Usuarios.AnyAsync(u => u.CorreoElectronico == usuario.CorreoElectronico);
                    if (existeCorreo)
                    {
                        ModelState.AddModelError("CorreoElectronico", "Este correo ya está registrado en el sistema.");
                        return View(usuario);
                    }

                    // 2. Automatizar campos internos
                    usuario.FechaRegistro = DateTime.Now; // Toma la fecha y hora actual
                    usuario.EstadoActivo = true; // Todo usuario nuevo nace activo

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "¡Usuario registrado correctamente!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }
            return View(usuario);
        }


        // EDIT (Editar e Inhabilitar Usuario)

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Para evitar que la fecha de registro original se pierda al editar,
                    // la buscamos en la base de datos para mantenerla intacta.
                    var usuarioOriginal = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);
                    if (usuarioOriginal != null)
                    {
                        usuario.FechaRegistro = usuarioOriginal.FechaRegistro;
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Usuario actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                }
            }
            return View(usuario);
        }
    }
}