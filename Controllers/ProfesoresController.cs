using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;
using ControlEscolarApp.Models;
using ControlEscolarApp.Filters;

namespace ControlEscolarApp.Controllers
{
    [SessionAuthorize("Admin", "Profesor", "Alumno")]
    public class ProfesoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfesoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var profesores = await _context.Profesores.ToListAsync();
            return View(profesores);
        }

        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(Profesor profesor)
        {
            if (ModelState.IsValid)
            {
                _context.Profesores.Add(profesor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(profesor);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null) return NotFound();

            return View(profesor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, Profesor profesor)
        {
            if (id != profesor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(profesor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(profesor);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var profesor = await _context.Profesores.FirstOrDefaultAsync(p => p.Id == id);
            if (profesor == null) return NotFound();

            return View(profesor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor != null)
            {
                _context.Profesores.Remove(profesor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}