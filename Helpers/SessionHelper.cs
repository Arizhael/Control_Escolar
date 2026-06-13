using Microsoft.AspNetCore.Http;

namespace ControlEscolarApp.Helpers
{
    public static class SessionHelper
    {
        public static bool IsLoggedIn(HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.Session.GetString("Username"));
        }

        public static string? GetRol(HttpContext httpContext)
        {
            return httpContext.Session.GetString("Rol");
        }

        public static bool IsAdmin(HttpContext httpContext)
        {
            return GetRol(httpContext) == "Admin";
        }

        public static bool IsProfesor(HttpContext httpContext)
        {
            return GetRol(httpContext) == "Profesor";
        }

        public static bool IsAlumno(HttpContext httpContext)
        {
            return GetRol(httpContext) == "Alumno";
        }
    }
}