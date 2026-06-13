using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;
using ControlEscolarApp.Models;
using ControlEscolarApp.Filters;

namespace ControlEscolarApp.Controllers
{
    [SessionAuthorize("Admin", "Profesor", "Alumno")]
    public class MateriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MateriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var materias = await _context.Materias.ToListAsync();
            return View(materias);
        }

        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Materias.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(materia);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return NotFound();

            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, Materia materia)
        {
            if (id != materia.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(materia);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var materia = await _context.Materias.FirstOrDefaultAsync(m => m.Id == id);
            if (materia == null) return NotFound();

            return View(materia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia != null)
            {
                _context.Materias.Remove(materia);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}