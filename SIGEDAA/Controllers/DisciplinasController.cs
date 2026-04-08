using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    [Authorize]
    public class DisciplinasController : Controller
    {
        // SIN private readonly
        public AppDbContext _context;

        public DisciplinasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Cambiado a ToArrayAsync()
            IEnumerable<Disciplina> disciplinas = await _context.Disciplinas.ToArrayAsync();
            return View(disciplinas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.IdDisciplina == id);

            if (disciplina == null) return NotFound();

            return View(disciplina);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disciplina disciplina)
        {
            // Solo protegemos la descripción si la dejan en blanco
            if (string.IsNullOrEmpty(disciplina.DescripcionReglas)) disciplina.DescripcionReglas = "N/A";

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
                    string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Error en la base de datos: " + errorMsg);
                }
            }
            return View(disciplina);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var disciplina = await _context.Disciplinas.FindAsync(id);

            if (disciplina == null) return NotFound();

            return View(disciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Disciplina disciplina)
        {
            if (id != disciplina.IdDisciplina) return NotFound();

            if (string.IsNullOrEmpty(disciplina.DescripcionReglas)) disciplina.DescripcionReglas = "N/A";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplina);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Disciplina actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Error al actualizar: " + errorMsg);
                }
            }
            return View(disciplina);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.IdDisciplina == id);

            if (disciplina == null) return NotFound();

            return View(disciplina);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disciplina = await _context.Disciplinas.FindAsync(id);
            if (disciplina != null)
            {
                try
                {
                    _context.Disciplinas.Remove(disciplina);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "La disciplina fue eliminada permanentemente.";
                }
                catch (Exception)
                {
                    TempData["Error"] = "No se puede eliminar esta disciplina porque ya hay competencias o atletas asociados a ella.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}