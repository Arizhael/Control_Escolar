using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;
using ControlEscolarApp.Models;
using ControlEscolarApp.Filters;

namespace ControlEscolarApp.Controllers
{
    public class CalificacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalificacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [SessionAuthorize("Admin", "Profesor")]
        public async Task<IActionResult> Index()
        {
            var calificaciones = _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Materia)
                .Include(c => c.Grupo)
                .Include(c => c.Profesor);

            return View(await calificaciones.ToListAsync());
        }

        [SessionAuthorize("Admin", "Profesor")]
        public IActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin", "Profesor")]
        public async Task<IActionResult> Create(Calificacion calificacion)
        {
            bool yaExiste = await _context.Calificaciones.AnyAsync(c =>
                c.AlumnoId == calificacion.AlumnoId &&
                c.MateriaId == calificacion.MateriaId &&
                c.GrupoId == calificacion.GrupoId);

            if (yaExiste)
            {
                ModelState.AddModelError(string.Empty, "Ya existe una calificación registrada para ese alumno en esa materia y grupo.");
            }

            if (ModelState.IsValid)
            {
                calificacion.Final = (calificacion.Parcial1 + calificacion.Parcial2 + calificacion.Parcial3) / 3;
                _context.Calificaciones.Add(calificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(calificacion);
            return View(calificacion);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var calificacion = await _context.Calificaciones.FindAsync(id);
            if (calificacion == null) return NotFound();

            CargarCombos(calificacion);
            return View(calificacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, Calificacion calificacion)
        {
            if (id != calificacion.Id) return NotFound();

            bool yaExiste = await _context.Calificaciones.AnyAsync(c =>
                c.Id != calificacion.Id &&
                c.AlumnoId == calificacion.AlumnoId &&
                c.MateriaId == calificacion.MateriaId &&
                c.GrupoId == calificacion.GrupoId);

            if (yaExiste)
            {
                ModelState.AddModelError(string.Empty, "Ya existe una calificación registrada para ese alumno en esa materia y grupo.");
            }

            if (ModelState.IsValid)
            {
                calificacion.Final = (calificacion.Parcial1 + calificacion.Parcial2 + calificacion.Parcial3) / 3;
                _context.Update(calificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(calificacion);
            return View(calificacion);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var calificacion = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Materia)
                .Include(c => c.Grupo)
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (calificacion == null) return NotFound();

            return View(calificacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calificacion = await _context.Calificaciones.FindAsync(id);
            if (calificacion != null)
            {
                _context.Calificaciones.Remove(calificacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [SessionAuthorize("Admin", "Alumno")]
        public IActionResult BuscarAlumno()
        {
            return View();
        }

        [HttpPost]
        [SessionAuthorize("Admin", "Alumno")]
        public async Task<IActionResult> BuscarAlumno(string numeroBoleta)
        {
            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.NumeroBoleta == numeroBoleta);

            if (alumno == null)
            {
                ViewBag.Mensaje = "No se encontró un alumno con ese número de boleta.";
                return View();
            }

            var calificaciones = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Materia)
                .Include(c => c.Grupo)
                .Include(c => c.Profesor)
                .Where(c => c.AlumnoId == alumno.Id)
                .ToListAsync();

            ViewBag.AlumnoNombre = alumno.Nombre + " " + alumno.ApellidoPaterno;
            ViewBag.NumeroBoleta = alumno.NumeroBoleta;

            return View("ResultadoAlumno", calificaciones);
        }

        private void CargarCombos(Calificacion? calificacion = null)
        {
            ViewBag.Alumnos = new SelectList(_context.Alumnos, "Id", "NumeroBoleta", calificacion?.AlumnoId);
            ViewBag.Materias = new SelectList(_context.Materias, "Id", "Nombre", calificacion?.MateriaId);
            ViewBag.Grupos = new SelectList(_context.Grupos, "Id", "Nombre", calificacion?.GrupoId);
            ViewBag.Profesores = new SelectList(_context.Profesores, "Id", "Nombre", calificacion?.ProfesorId);
        }
    }
}