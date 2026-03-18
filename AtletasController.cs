using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Threading.Tasks;
using System.Linq;

namespace SIGEDAA.Controllers
{
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
            var atletas = await _context.Atletas
                .Include(a => a.IdClub == 0 ? null : null) // placeholder: clubs se obtienen por IdClub manualmente en la vista si se requiere
                .ToListAsync();
            return View(atletas);
        }

        // GET: /Atletas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta == null) return NotFound();

            return View(atleta);
        }

        // GET: /Atletas/Create
        public IActionResult Create()
        {
            ViewBag.Clubes = new SelectList(_context.Clubes.ToList(), "IdClub", "NombreClub");
            return View();
        }

        // POST: /Atletas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Atleta atleta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(atleta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clubes = new SelectList(_context.Clubes.ToList(), "IdClub", "NombreClub", atleta.IdClub);
            return View(atleta);
        }

        // GET: /Atletas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta == null) return NotFound();

            ViewBag.Clubes = new SelectList(_context.Clubes.ToList(), "IdClub", "NombreClub", atleta.IdClub);
            return View(atleta);
        }

        // POST: /Atletas/Edit/5
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Atletas.Any(e => e.IdAtleta == atleta.IdAtleta))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clubes = new SelectList(_context.Clubes.ToList(), "IdClub", "NombreClub", atleta.IdClub);
            return View(atleta);
        }

        // GET: /Atletas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta == null) return NotFound();

            return View(atleta);
        }

        // POST: /Atletas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta != null)
            {
                _context.Atletas.Remove(atleta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}