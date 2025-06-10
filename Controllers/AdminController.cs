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


        // --------------------------
        // CRUD aLUMNOS
        // -----

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

        // --------------------------
        // CRUD PROYECTOS
        // --------------------------

        [HttpGet]
        public IActionResult Proyectos()
        {
            var proyectos = _context.Proyectos
                .Include(p => p.Asesor)
                .Include(p => p.AlumnosProyectos)
                    .ThenInclude(ap => ap.Alumno)
                .ToList();

            return View("Proyectos", proyectos);
        }

        [HttpGet]
        public IActionResult CreateProyecto()
        {
            // Carga los datos necesarios para dropdowns y buscador
            ViewBag.Asesores = _context.Asesores.ToList();
            ViewBag.Alumnos = _context.Alumnos.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProyecto(
     [Bind("Nombre,Descripcion,RazonSocial,Estado,FechaInicio,FechaFin,AsesorId")] Proyecto proyecto,
     string AlumnosMatriculas,
     List<IFormFile> ArchivosAdjuntos)
        {
            if (proyecto.Estado != "No iniciado")
            {
                if (!proyecto.FechaInicio.HasValue)
                    ModelState.AddModelError("FechaInicio", "La fecha de inicio es obligatoria.");

                if (!proyecto.FechaFin.HasValue)
                    ModelState.AddModelError("FechaFin", "La fecha de fin es obligatoria.");

                if (!proyecto.AsesorId.HasValue)
                    ModelState.AddModelError("AsesorId", "Debe seleccionar un asesor.");

                if (string.IsNullOrWhiteSpace(AlumnosMatriculas))
                    ModelState.AddModelError("AlumnosMatriculas", "Debe agregar al menos un alumno.");
            }
            else
            {
                // Este sí garantiza que no bloquee aunque no exista el campo
                ModelState.Remove("AlumnosMatriculas");
            }

            if (!ModelState.IsValid)
            {
                foreach (var kv in ModelState)
                    foreach (var err in kv.Value.Errors)
                        Console.WriteLine($"Error en {kv.Key}: {err.ErrorMessage}");

                ViewBag.Asesores = _context.Asesores.ToList();
                ViewBag.Alumnos = _context.Alumnos.ToList();
                return View(proyecto);
            }

            // Guardar proyecto para obtener su Id
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();

            // Asociar alumnos seleccionados
            var matriculas = AlumnosMatriculas?
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Trim())
                ?? Array.Empty<string>();

            foreach (var matricula in matriculas)
            {
                _context.AlumnosProyectos.Add(new AlumnoProyecto
                {
                    ProyectoId = proyecto.Id,
                    AlumnoMatricula = matricula
                });
            }

            // Crear carpeta física
            var folder = Path.Combine("wwwroot", "proyectos", proyecto.Id.ToString());
            Directory.CreateDirectory(folder);

            // Guardar cada archivo adjunto
            if (ArchivosAdjuntos != null && ArchivosAdjuntos.Any())
            {
                foreach (var archivo in ArchivosAdjuntos)
                {
                    var extension = Path.GetExtension(archivo.FileName).ToLower();
                    var path = Path.Combine(folder, archivo.FileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await archivo.CopyToAsync(stream);

                    _context.Archivos.Add(new Archivo
                    {
                        Nombre = archivo.FileName,
                        Extension = extension,
                        RutaArchivo = $"/proyectos/{proyecto.Id}/{archivo.FileName}",
                        ProyectoId = proyecto.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Proyectos");
        }

        [HttpGet]
        public IActionResult DetailsProyecto(int id)
        {
            var proyecto = _context.Proyectos
                .Include(p => p.Asesor)
                .Include(p => p.AlumnosProyectos)
                    .ThenInclude(ap => ap.Alumno)
                .Include(p => p.Archivos)
                .FirstOrDefault(p => p.Id == id);

            if (proyecto == null)
                return NotFound();

            return View("DetailsProyecto", proyecto);
        }

        [HttpGet]
        public IActionResult Archivos(int id)
        {
            var proyecto = _context.Proyectos
                .Include(p => p.Archivos)
                .FirstOrDefault(p => p.Id == id);

            if (proyecto == null)
                return NotFound();

            return View("Archivos", proyecto);
        }

        [HttpGet]
        public async Task<IActionResult> EditProyecto(int? id)
        {
            if (id == null) return NotFound();

            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null) return NotFound();

            // Carga los datos necesarios para dropdowns y buscador
            ViewBag.Asesores = await _context.Asesores.ToListAsync();
            ViewBag.Alumnos = await _context.Alumnos.ToListAsync();

            return View(proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProyecto(int id, [Bind("Id,Nombre,Descripcion,RazonSocial")] Proyecto proyecto)
        {
            if (id != proyecto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proyecto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Proyectos.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // En caso de error, se vuelve a cargar la información para la vista
            ViewBag.Asesores = await _context.Asesores.ToListAsync();
            ViewBag.Alumnos = await _context.Alumnos.ToListAsync();

            return View(proyecto);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProyecto(int? id)
        {
            if (id == null) return BadRequest();

            var proyecto = await _context.Proyectos
                .Include(p => p.Asesor)
                .Include(p => p.AlumnosProyectos)
                    .ThenInclude(ap => ap.Alumno)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proyecto == null) return NotFound();

            return View(proyecto);
        }

        [HttpPost, ActionName("DeleteProyecto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProyectoConfirmed(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                _context.Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Proyectos"); // Asegúrate que exista una vista Index
        }

        //Crud Graficas//

        [HttpGet]
        public IActionResult ProyectosDashboard()
        {
        
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDatosDashboard(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var proyectos = _context.Proyectos.AsQueryable();

            if (fechaInicio.HasValue)
                proyectos = proyectos.Where(p => p.FechaInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                proyectos = proyectos.Where(p => p.FechaInicio <= fechaFin.Value);

            var datos = await proyectos
                .GroupBy(p => p.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var estados = new[] { "No iniciado", "En curso", "Finalizado" };
            var result = new
            {
                labels = estados,
                counts = estados.Select(e => datos.FirstOrDefault(d => d.Estado == e)?.Cantidad ?? 0)
            };

            return Json(result);
        }

        // Para el modal (HTML)
        [HttpGet]
        public async Task<IActionResult> GetProyectosPorEstado(string estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var proyectos = _context.Proyectos
                .Include(p => p.Asesor)
                .Include(p => p.AlumnosProyectos).ThenInclude(ap => ap.Alumno)
                .Where(p => p.Estado == estado);

            if (fechaInicio.HasValue)
                proyectos = proyectos.Where(p => p.FechaInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                proyectos = proyectos.Where(p => p.FechaInicio <= fechaFin.Value);

            var lista = await proyectos.ToListAsync();
            return PartialView("ListaProyectosModal", lista);
        }

        [HttpGet]
        public IActionResult DetalleProyectoModal(int id)
        {
            var proyecto = _context.Proyectos
                .Include(p => p.Asesor)
                .Include(p => p.AlumnosProyectos).ThenInclude(ap => ap.Alumno)
                .FirstOrDefault(p => p.Id == id);

            if (proyecto == null) return NotFound();

            return PartialView("DetalleProyectoModal", proyecto);
        }
        public IActionResult ExportarProyectosPorEstadoExcel(string estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var proyectos = _context.Proyectos
                .Include(p => p.Asesor)
                .Include(p => p.AlumnosProyectos).ThenInclude(ap => ap.Alumno)
                .Where(p => p.Estado == estado);

            if (fechaInicio.HasValue)
                proyectos = proyectos.Where(p => p.FechaInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                proyectos = proyectos.Where(p => p.FechaInicio <= fechaFin.Value);

            var lista = proyectos.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Proyecto,Descripción,Razón Social,Asesor,Fecha Inicio,Fecha Fin,Alumnos");

            foreach (var p in lista)
            {
                var asesor = p.Asesor != null ? $"{p.Asesor.Nombre} {p.Asesor.ApellidoPaterno}" : "";
                var alumnos = p.AlumnosProyectos?
                    .Select(ap => $"{ap.Alumno.Matricula} - {ap.Alumno.Nombre} {ap.Alumno.ApellidoPaterno}")
                    .ToList();

                var alumnosConcatenados = alumnos != null && alumnos.Any()
                    ? string.Join(" | ", alumnos)
                    : "Sin alumnos";

                csv.AppendLine($"\"{p.Nombre}\",\"{p.Descripcion}\",\"{p.RazonSocial}\",\"{asesor}\",\"{p.FechaInicio?.ToShortDateString()}\",\"{p.FechaFin?.ToShortDateString()}\",\"{alumnosConcatenados}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"Proyectos_{estado}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }

    }


}
