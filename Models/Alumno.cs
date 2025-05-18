using System.ComponentModel.DataAnnotations;

namespace UserRoles.Models
{
    public class Alumno
    {
        [Key]
        [Required]
        public string Matricula { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        [Required]
        public string ApellidoMaterno { get; set; }

        [Required]
        public string Departamento { get; set; }

        [Required]
        [Phone]
        public string NumeroContacto { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        public List<AlumnoProyecto> AlumnosProyectos { get; set; }
    }
}
