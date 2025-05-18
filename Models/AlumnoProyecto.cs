using UserRoles.Models;

namespace UserRoles.Models
{
    public class AlumnoProyecto
    {
        public string AlumnoMatricula { get; set; }
        public Alumno Alumno { get; set; }

        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}
