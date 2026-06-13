using System.ComponentModel.DataAnnotations;

namespace ControlEscolarApp.Models
{
    public class Alumno
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Número de Boleta")]
        public string NumeroBoleta { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Display(Name = "Apellido Materno")]
        public string? ApellidoMaterno { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;
    }
}