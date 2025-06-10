using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nodo.ViewModels;
using UserRoles.Models;


namespace Nodo.Controllers
{
    public class Usuario : Controller
    {
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
