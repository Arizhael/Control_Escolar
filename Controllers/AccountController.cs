using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Data;

namespace ControlEscolarApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            HttpContext.Session.SetString("Username", usuario.Username);
            HttpContext.Session.SetString("Rol", usuario.Rol);
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            if (usuario.AlumnoId.HasValue)
                HttpContext.Session.SetInt32("AlumnoId", usuario.AlumnoId.Value);

            if (usuario.ProfesorId.HasValue)
                HttpContext.Session.SetInt32("ProfesorId", usuario.ProfesorId.Value);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}