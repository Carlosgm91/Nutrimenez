using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nutrimenez.Data;
using Nutrimenez.Models;

namespace Nutrimenez.Controllers
{
    [Authorize(Roles = "Usuario")]
    public class MisDatosController : Controller
    {
        private readonly NutrimenezContexto _context;
        public MisDatosController(NutrimenezContexto context)
        {
            _context = context;
        }
        // GET: MisDatos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MisDatos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
       Create([Bind("Id,Nombre,Email,Telefono,FechaNacimiento")] Usuario usuario)
        {
            // Asignar el Email del usuario
            usuario.Email = User.Identity.Name;
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        // GET: MisDatos/Edit
        public async Task<IActionResult> Edit()
        {
            // Se seleccioanan datos del usuario correspondiente al emplead actual
            string emailUsuario = User.Identity.Name;
            Usuario usuario = await _context.Usuarios
            .Where(e => e.Email == emailUsuario)
            .FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }
        // POST: MisDatos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
         [Bind("Id,Nombre,Email,Telefono,FechaNacimiento")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }

}
