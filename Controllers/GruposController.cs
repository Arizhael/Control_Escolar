using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;
using ControlEscolarApp.Models;
using ControlEscolarApp.Filters;

namespace ControlEscolarApp.Controllers
{
    [SessionAuthorize("Admin", "Profesor", "Alumno")]
    public class GruposController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GruposController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var grupos = await _context.Grupos.ToListAsync();
            return View(grupos);
        }

        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                _context.Grupos.Add(grupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(grupo);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            return View(grupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, Grupo grupo)
        {
            if (id != grupo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(grupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(grupo);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FirstOrDefaultAsync(g => g.Id == id);
            if (grupo == null) return NotFound();

            return View(grupo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo != null)
            {
                _context.Grupos.Remove(grupo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}