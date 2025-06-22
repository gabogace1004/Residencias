using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserRoles.Models

{
    public class Sugerencias
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string ApellidoMaterno { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public string Matricula { get; set; }
        [Required]
        public string NumeroContacto { get; set; }
        [Required]
        public string Correo { get; set; }




        [Required]
        [Display(Name = "Sugerencia de mejora")]
        public string NombreProyecto { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string RazonSocial { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
