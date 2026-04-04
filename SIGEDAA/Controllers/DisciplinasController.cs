using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class DisciplinasController : Controller
    {
        private readonly AppDbContext _context;

        public DisciplinasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Disciplinas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Disciplinas.ToListAsync());
        }

        // GET: /Disciplinas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Disciplinas/Create (El código blindado)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disciplina disciplina)
        {
            // 1. Borramos TODAS las validaciones estrictas y ocultas
            ModelState.Clear();

            // 2. Validamos nosotros a mano
            if (string.IsNullOrWhiteSpace(disciplina.NombreDisciplina))
            {
                ModelState.AddModelError("NombreDisciplina", "El nombre de la disciplina es obligatorio.");
            }

            // 3. Si pasa nuestra validación manual, lo obligamos a guardar
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(disciplina);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "¡Disciplina creada correctamente!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Si la base de datos se queja, mostramos el error
                    ModelState.AddModelError("", "Error en la base de datos: " + ex.Message);
                }
            }

            return View(disciplina);
        }

        // GET: /Disciplinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var disciplina = await _context.Disciplinas.FindAsync(id);
            return disciplina == null ? NotFound() : View(disciplina);
        }

        // POST: /Disciplinas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Disciplina disciplina)
        {
            if (id != disciplina.IdDisciplina) return NotFound();

            ModelState.Clear(); // Limpiamos también aquí por si acaso

            if (string.IsNullOrWhiteSpace(disciplina.NombreDisciplina))
            {
                ModelState.AddModelError("NombreDisciplina", "El nombre de la disciplina es obligatorio.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplina);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Disciplina actualizada.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                }
            }
            return View(disciplina);
        }
    }
}