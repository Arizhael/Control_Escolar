using Microsoft.EntityFrameworkCore;
using ControlEscolarApp.Models;

namespace ControlEscolarApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<AsignacionMateria> AsignacionesMateria { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Calificacion>()
                .Property(c => c.Parcial1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Calificacion>()
                .Property(c => c.Parcial2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Calificacion>()
                .Property(c => c.Parcial3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Calificacion>()
                .Property(c => c.Final)
                .HasPrecision(5, 2);
        }
    }
}