using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using UserRoles.Models;

namespace UserRoles.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Asesor> Asesores { get; set; } // ← ESTA LÍNEA AGREGA LA TABLA
        public DbSet<Alumno> Alumnos { get; set; }// ← ESTA LÍNEA AGREGA LA TABLA
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<AlumnoProyecto> AlumnosProyectos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AlumnoProyecto>()
                .HasKey(ap => new { ap.AlumnoMatricula, ap.ProyectoId });

            modelBuilder.Entity<AlumnoProyecto>()
                .HasOne(ap => ap.Alumno)
                .WithMany(a => a.AlumnosProyectos)
                .HasForeignKey(ap => ap.AlumnoMatricula);

            modelBuilder.Entity<AlumnoProyecto>()
                .HasOne(ap => ap.Proyecto)
                .WithMany(p => p.AlumnosProyectos)
                .HasForeignKey(ap => ap.ProyectoId);
        }
    }
}