using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;
using ControlEscolarApp.Models;
using ControlEscolarApp.Filters;

namespace ControlEscolarApp.Controllers
{
    [SessionAuthorize("Admin", "Profesor", "Alumno")]
    public class AsignacionesMateriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignacionesMateriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var asignaciones = _context.AsignacionesMateria
                .Include(a => a.Materia)
                .Include(a => a.Profesor)
                .Include(a => a.Grupo);

            return View(await asignaciones.ToListAsync());
        }

        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre");
            ViewBag.Profesores = new SelectList(_context.Profesores, "Id", "Nombre");
            ViewBag.Grupos = new SelectList(_context.Grupos, "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(AsignacionMateria asignacion)
        {
            if (ModelState.IsValid)
            {
                _context.AsignacionesMateria.Add(asignacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre", asignacion.MateriaId);
            ViewBag.Profesores = new SelectList(_context.Profesores, "Id", "Nombre", asignacion.ProfesorId);
            ViewBag.Grupos = new SelectList(_context.Grupos, "Id", "Nombre", asignacion.GrupoId);

            return View(asignacion);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var asignacion = await _context.AsignacionesMateria.FindAsync(id);
            if (asignacion == null) return NotFound();

            ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre", asignacion.MateriaId);
            ViewBag.Profesores = new SelectList(_context.Profesores, "Id", "Nombre", asignacion.ProfesorId);
            ViewBag.Grupos = new SelectList(_context.Grupos, "Id", "Nombre", asignacion.GrupoId);

            return View(asignacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, AsignacionMateria asignacion)
        {
            if (id != asignacion.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(asignacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre", asignacion.MateriaId);
            ViewBag.Profesores = new SelectList(_context.Profesores, "Id", "Nombre", asignacion.ProfesorId);
            ViewBag.Grupos = new SelectList(_context.Grupos, "Id", "Nombre", asignacion.GrupoId);

            return View(asignacion);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var asignacion = await _context.AsignacionesMateria
                .Include(a => a.Materia)
                .Include(a => a.Profesor)
                .Include(a => a.Grupo)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asignacion == null) return NotFound();

            return View(asignacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignacion = await _context.AsignacionesMateria.FindAsync(id);
            if (asignacion != null)
            {
                _context.AsignacionesMateria.Remove(asignacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}