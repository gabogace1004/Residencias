using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nodo.ViewModels;

namespace Nodo.Controllers
{
   
    
        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            // Vista principal del panel admin
            public IActionResult Index()
            {
                return View();
            }

        // Gestión de proyectos
        public IActionResult Prueba()
        {
            var proyectos = new List<ProyectoNodoViewModel>
    {
        new ProyectoNodoViewModel
        {
            Id = 1,
            Nombre = "Gestión de Órdenes Nodo",
            Alumnos = new List<string> { "Juan Silva", "José Medina" },
            Asesor = "Mtro. Vargas",
            Estado = "En curso",
            FechaInicio = new DateTime(2025, 5, 1),
            FechaFin = new DateTime(2025, 6, 30)
        },
        new ProyectoNodoViewModel
        {
            Id = 2,
            Nombre = "Control de Calidad Web",
            Alumnos = new List<string> { "Laura Torres", "Carlos Pérez" },
            Asesor = "Ing. Romero",
            Estado = "Finalizado",
            FechaInicio = new DateTime(2024, 9, 15),
            FechaFin = new DateTime(2024, 12, 15)
        }
    };

            return View(proyectos);
        }


        // Gestión de usuarios (placeholder)
        [Authorize(Roles = "Admin")]
        public IActionResult Admin() => View();

        private static List<AsesorViewModel> asesores = new();

        [HttpGet]
        public IActionResult Asesores()
        {
            return View(asesores);
        }

        [HttpGet]
        public IActionResult CreateAsesor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAsesor(AsesorViewModel asesor)
        {
            asesor.Id = asesores.Count + 1;
            asesores.Add(asesor);
            return RedirectToAction("Asesores");
        }

    }


    }
    
