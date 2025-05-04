namespace Nodo.ViewModels
{
    public class ProyectoNodoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<string> Alumnos { get; set; }
        public string Asesor { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
