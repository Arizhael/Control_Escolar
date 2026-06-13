using System.ComponentModel.DataAnnotations;

namespace ControlEscolarApp.Models
{
    public class Materia
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Clave")]
        public string Clave { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nombre de la Materia")]
        public string Nombre { get; set; } = string.Empty;
    }
}