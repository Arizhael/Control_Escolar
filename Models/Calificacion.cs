using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlEscolarApp.Models
{
    public class Calificacion
    {
        public int Id { get; set; }

        [Display(Name = "Alumno")]
        public int AlumnoId { get; set; }

        [Display(Name = "Materia")]
        public int MateriaId { get; set; }

        [Display(Name = "Grupo")]
        public int GrupoId { get; set; }

        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }

        [Range(0, 10)]
        public decimal Parcial1 { get; set; }

        [Range(0, 10)]
        public decimal Parcial2 { get; set; }

        [Range(0, 10)]
        public decimal Parcial3 { get; set; }

        [Display(Name = "Calificación Final")]
        public decimal Final { get; set; }

        [ForeignKey("AlumnoId")]
        public Alumno? Alumno { get; set; }

        [ForeignKey("MateriaId")]
        public Materia? Materia { get; set; }

        [ForeignKey("GrupoId")]
        public Grupo? Grupo { get; set; }

        [ForeignKey("ProfesorId")]
        public Profesor? Profesor { get; set; }
    }
}