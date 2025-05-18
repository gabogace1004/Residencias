namespace UserRoles.Models
{
    public class Asesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Departamento { get; set; }
        public string NumeroContacto { get; set; }
        public string Correo { get; set; }

        public List<Proyecto> Proyectos { get; set; }
    }
}
