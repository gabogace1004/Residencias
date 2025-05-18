
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserRoles.Models
{
    public class Proyecto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RazonSocial { get; set; }
        public string Estado { get; set; }  // No iniciado, En curso, Finalizado

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        [ValidateNever]
        public int? AsesorId { get; set; }

        [ValidateNever]
        public Asesor Asesor { get; set; }

        [ValidateNever]
        public List<AlumnoProyecto> AlumnosProyectos { get; set; } = new();

        [ValidateNever]
        public List<Archivo> Archivos { get; set; } = new();
    }
}

