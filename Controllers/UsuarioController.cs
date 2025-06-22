using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRoles.Data;
using UserRoles.Models;

namespace Nodo.Controllers
{
    [Authorize(Roles = "User")]
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Vista inicial
        [HttpGet]
        public IActionResult User()
        {
            return View(); // Views/Usuario/Index.cshtml
        }

        // Mostrar formulario de sugerencia
        [HttpGet]
        public IActionResult CrearSugerencia()
        {
            return View(); // Views/Usuario/CrearSugerencia.cshtml
        }

        // Guardar sugerencia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Sugerencias model)
        {
            if (!ModelState.IsValid)
                return View("CrearSugerencia", model);

            model.FechaRegistro = DateTime.Now;

            _context.Sugerencias.Add(model);
            _context.SaveChanges();

            ViewBag.Mensaje = "✅ ¡Tu sugerencia fue enviada con éxito!";
            ModelState.Clear();

            return View("CrearSugerencia");
        }
    }
}
