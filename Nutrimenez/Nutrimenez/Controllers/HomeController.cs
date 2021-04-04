using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nutrimenez.Data;
using Nutrimenez.Models;

namespace Nutrimenez.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NutrimenezContexto _context;

        public HomeController(ILogger<HomeController> logger, NutrimenezContexto context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Busca empleado correspondiente al usuario actual. Si existe 
            // va a View y en caso contrario, va a crear el empleado.
            string emailUsuario = User.Identity.Name;
            Usuario usuario = _context.Usuarios.Where(e => e.Email == emailUsuario)
                                               .FirstOrDefault();
            if (User.Identity.IsAuthenticated && User.IsInRole("Usuario") && usuario == null)
            {
                return RedirectToAction("Create", "MisDatos");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
