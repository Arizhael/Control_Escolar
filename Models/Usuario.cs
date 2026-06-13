using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlEscolarApp.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty; // Admin, Profesor, Alumno

        [Display(Name = "Alumno")]
        public int? AlumnoId { get; set; }

        [Display(Name = "Profesor")]
        public int? ProfesorId { get; set; }

        [ForeignKey("AlumnoId")]
        public Alumno? Alumno { get; set; }

        [ForeignKey("ProfesorId")]
        public Profesor? Profesor { get; set; }
    }
}