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

        // ==========================================
        // INDEX (Lista)
        // ==========================================
        public async Task<IActionResult> Index()
        {
            return View(await _context.Disciplinas.ToListAsync());
        }

        // ==========================================
        // DETAILS (Ver Detalles)
        // ==========================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.IdDisciplina == id);

            if (disciplina == null) return NotFound();

            return View(disciplina);
        }

        // ==========================================
        // CREATE (Crear)
        // ==========================================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disciplina disciplina)
        {
            // 1. Asignamos valores por defecto a los campos que la vista no envía
            if (string.IsNullOrEmpty(disciplina.GeneroPermitido)) disciplina.GeneroPermitido = "No especificado";
            if (string.IsNullOrEmpty(disciplina.DescripcionReglas)) disciplina.DescripcionReglas = "N/A";

            // 2. Quitamos estos campos de la validación porque la vista no los mandó
            ModelState.Remove("GeneroPermitido");
            ModelState.Remove("DescripcionReglas");

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
                    // Si falla la DB, extraemos el error real para verlo en pantalla
                    string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Error en la base de datos: " + errorMsg);
                }
            }

            return View(disciplina);
        }

        // ==========================================
        // EDIT (Editar)
        // ==========================================
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

            // Hacemos el mismo truco para la edición
            if (string.IsNullOrEmpty(disciplina.GeneroPermitido)) disciplina.GeneroPermitido = "No especificado";
            if (string.IsNullOrEmpty(disciplina.DescripcionReglas)) disciplina.DescripcionReglas = "N/A";

            ModelState.Remove("GeneroPermitido");
            ModelState.Remove("DescripcionReglas");

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
        // ==========================================
        // DELETE (Confirmación de Eliminar)
        // ==========================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var disciplina = await _context.Disciplinas
                .FirstOrDefaultAsync(m => m.IdDisciplina == id);

            if (disciplina == null) return NotFound();

            return View(disciplina);
        }

        // POST: /Disciplinas/Delete/5
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
                    // Si SQL Server bloquea la eliminación (porque la disciplina ya tiene resultados)
                    TempData["Error"] = "No se puede eliminar esta disciplina porque ya hay competencias o atletas asociados a ella.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }

}