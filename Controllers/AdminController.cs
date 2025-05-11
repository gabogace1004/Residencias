using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoles.Data;
using UserRoles.Models;
using Nodo.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Http;


namespace Nodo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Vista principal del panel admin (cuando haces login como admin)
        [HttpGet]
        public IActionResult Admin()
        {
            return View(); // Asegúrate de tener Views/Admin/Admin.cshtml
        }

        // Simulación de proyectos del Nodo
        [HttpGet]
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

        // --------------------------
        // CRUD ASESORES
        // --------------------------

        // Mostrar lista de asesores
        [HttpGet]
        public IActionResult Asesores()
        {
            var asesores = _context.Asesores.ToList();
            return View(asesores);
        }

        // Formulario para crear asesor
        [HttpGet]
        public IActionResult CreateAsesor()
        {
            return View();
        }

        // Guardar asesor nuevo en BD
        [HttpPost]
        public IActionResult CreateAsesor(Asesor asesor)
        {
            bool correoDuplicado = _context.Asesores.Any(a => a.Correo == asesor.Correo);
            bool nombreDuplicado = _context.Asesores.Any(a =>
                a.Nombre == asesor.Nombre &&
                a.ApellidoPaterno == asesor.ApellidoPaterno &&
                a.ApellidoMaterno == asesor.ApellidoMaterno);

            if (correoDuplicado)
                ModelState.AddModelError("Correo", "Ya existe un asesor con este correo electrónico.");

            if (nombreDuplicado)
                ModelState.AddModelError("Nombre", "Ya existe un asesor con el mismo nombre completo.");

            if (ModelState.IsValid)
            {
                _context.Asesores.Add(asesor);
                _context.SaveChanges();
                return RedirectToAction("Asesores");
            }

            return View(asesor);
        }


        // Formulario para editar asesor
        [HttpGet]
        public IActionResult EditAsesor(int id)
        {
            var asesor = _context.Asesores.Find(id);
            if (asesor == null)
                return NotFound();

            return View(asesor);
        }

        // Guardar cambios de asesor
        [HttpPost]
        public IActionResult EditAsesor(Asesor asesor)
        {
            if (ModelState.IsValid)
            {
                _context.Asesores.Update(asesor);
                _context.SaveChanges();
                return RedirectToAction("Asesores");
            }

            return View(asesor);
        }

        // Confirmación para eliminar asesor
        [HttpGet]
        public IActionResult DeleteAsesor(int id)
        {
            var asesor = _context.Asesores.Find(id);
            if (asesor == null)
                return NotFound();

            return View(asesor);
        }

        // Eliminar asesor definitivamente
        [HttpPost, ActionName("DeleteAsesor")]
        public IActionResult DeleteAsesorConfirmed(int id)
        {
            var asesor = _context.Asesores.Find(id);
            if (asesor != null)
            {
                _context.Asesores.Remove(asesor);
                _context.SaveChanges();
            }

            return RedirectToAction("Asesores");
        }
        public IActionResult ExportAsesoresExcel()
        {
            var asesores = _context.Asesores.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("NombreCompleto,Departamento,NumeroContacto,Correo");

            foreach (var a in asesores)
            {
                csv.AppendLine($"{a.Nombre} {a.ApellidoPaterno} {a.ApellidoMaterno},{a.Departamento},{a.NumeroContacto},{a.Correo}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"asesores_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }


        public IActionResult Alumnos()
        {
            var alumnos = _context.Alumnos.ToList();
            return View("Alumnos", alumnos);
        }
        // Crear alumno
        [HttpGet]
        public IActionResult CreateAlumno()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAlumno(Alumno alumno)
        {
            if (_context.Alumnos.Any(a => a.Matricula == alumno.Matricula))
                ModelState.AddModelError("Matricula", "Ya existe un alumno con esta matrícula.");

            if (_context.Alumnos.Any(a => a.Correo == alumno.Correo))
                ModelState.AddModelError("Correo", "Ya existe un alumno con este correo.");

            if (ModelState.IsValid)
            {
                _context.Alumnos.Add(alumno);
                _context.SaveChanges();
                return RedirectToAction("Alumnos");
            }

            return View(alumno);
        }
        // Editar alumno
        [HttpGet]
        public IActionResult EditAlumno(string id)
        {
            var alumno = _context.Alumnos.Find(id);
            return alumno == null ? NotFound() : View(alumno);
        }

        [HttpPost]
        public IActionResult EditAlumno(Alumno alumno)
        {
            if (_context.Alumnos.Any(a => a.Matricula == alumno.Matricula))
                ModelState.AddModelError("Matricula", "Ya existe un alumno con esta matrícula.");

            if (_context.Alumnos.Any(a => a.Correo == alumno.Correo))
                ModelState.AddModelError("Correo", "Ya existe un alumno con este correo.");

            if (ModelState.IsValid)
            {
                _context.Alumnos.Update(alumno);
                _context.SaveChanges();
                return RedirectToAction("Alumnos");
            }

            return View(alumno);
        }

        // Eliminar alumno
        [HttpGet]
        public IActionResult DeleteAlumno(string id)
        {
            var alumno = _context.Alumnos.Find(id);
            return alumno == null ? NotFound() : View(alumno);
        }

        [HttpPost, ActionName("DeleteAlumno")]
        public IActionResult DeleteAlumnoConfirmed(string id)
        {
            var alumno = _context.Alumnos.Find(id);
            if (alumno != null)
            {
                _context.Alumnos.Remove(alumno);
                _context.SaveChanges();
            }

            return RedirectToAction("Alumnos");
        }
        public IActionResult ExportAlumnosExcel()
        {
            var alumnos = _context.Alumnos.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Matricula,Nombre,ApellidoPaterno,ApellidoMaterno,Departamento,NumeroContacto,Correo");

            foreach (var a in alumnos)
            {
                csv.AppendLine($"{a.Matricula},{a.Nombre},{a.ApellidoPaterno},{a.ApellidoMaterno},{a.Departamento},{a.NumeroContacto},{a.Correo}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"alumnos_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }

    }


}
