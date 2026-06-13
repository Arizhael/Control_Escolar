using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;
using ControlEscolarApp.Models;
using ControlEscolarApp.Filters;

namespace ControlEscolarApp.Controllers
{
    [SessionAuthorize("Admin")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Alumno)
                .Include(u => u.Profesor);

            return View(await usuarios.ToListAsync());
        }

        public IActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            bool usernameExiste = await _context.Usuarios.AnyAsync(u => u.Username == usuario.Username);

            if (usernameExiste)
            {
                ModelState.AddModelError(string.Empty, "Ese nombre de usuario ya existe.");
            }

            if (usuario.Rol == "Alumno" && !usuario.AlumnoId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Debes seleccionar un alumno para el rol Alumno.");
            }

            if (usuario.Rol == "Profesor" && !usuario.ProfesorId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Debes seleccionar un profesor para el rol Profesor.");
            }

            if (usuario.Rol == "Admin")
            {
                usuario.AlumnoId = null;
                usuario.ProfesorId = null;
            }

            if (usuario.Rol == "Alumno")
            {
                usuario.ProfesorId = null;
            }

            if (usuario.Rol == "Profesor")
            {
                usuario.AlumnoId = null;
            }

            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(usuario);
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            CargarCombos(usuario);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();

            bool usernameExiste = await _context.Usuarios.AnyAsync(u => u.Id != usuario.Id && u.Username == usuario.Username);

            if (usernameExiste)
            {
                ModelState.AddModelError(string.Empty, "Ese nombre de usuario ya existe.");
            }

            if (usuario.Rol == "Alumno" && !usuario.AlumnoId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Debes seleccionar un alumno para el rol Alumno.");
            }

            if (usuario.Rol == "Profesor" && !usuario.ProfesorId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Debes seleccionar un profesor para el rol Profesor.");
            }

            if (usuario.Rol == "Admin")
            {
                usuario.AlumnoId = null;
                usuario.ProfesorId = null;
            }

            if (usuario.Rol == "Alumno")
            {
                usuario.ProfesorId = null;
            }

            if (usuario.Rol == "Profesor")
            {
                usuario.AlumnoId = null;
            }

            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(usuario);
            return View(usuario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Alumno)
                .Include(u => u.Profesor)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void CargarCombos(Usuario? usuario = null)
        {
            ViewBag.Roles = new SelectList(new List<string> { "Admin", "Profesor", "Alumno" }, usuario?.Rol);
            ViewBag.Alumnos = new SelectList(_context.Alumnos, "Id", "NumeroBoleta", usuario?.AlumnoId);
            ViewBag.Profesores = new SelectList(_context.Profesores, "Id", "Nombre", usuario?.ProfesorId);
        }
    }
}