using System.ComponentModel.DataAnnotations;

namespace ControlEscolarApp.Models
{
    public class Grupo
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre del Grupo")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Semestre")]
        public int Semestre { get; set; }
    }
}