namespace UserRoles.Models
{
    public class Archivo
    {
        public int Id { get; set; }

        // Nombre visible
        public string Nombre { get; set; }

        // Tipo del archivo: "PDF", "ZIP"
        public string Extension { get; set; }

        // Ruta física en el servidor (wwwroot/proyectos/)
        public string RutaArchivo { get; set; }

        // Relación con Proyecto
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}
