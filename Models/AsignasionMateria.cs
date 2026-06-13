using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlEscolarApp.Models
{
    public class AsignacionMateria
    {
        public int Id { get; set; }

        [Display(Name = "Materia")]
        public int MateriaId { get; set; }

        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }

        [Display(Name = "Grupo")]
        public int GrupoId { get; set; }

        [ForeignKey("MateriaId")]
        public Materia? Materia { get; set; }

        [ForeignKey("ProfesorId")]
        public Profesor? Profesor { get; set; }

        [ForeignKey("GrupoId")]
        public Grupo? Grupo { get; set; }
    }
}