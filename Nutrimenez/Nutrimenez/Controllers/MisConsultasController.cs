using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nutrimenez.Data;
using Nutrimenez.Models;

namespace Nutrimenez.Controllers
{
    public class MisConsultasController : Controller
    {
        private readonly NutrimenezContexto _context;

        public MisConsultasController(NutrimenezContexto context)
        {
            _context = context;
        }

        // GET: MisConsultas
        public async Task<IActionResult> Index()
        {
            // Se selecciona el usuario correspondiente al usuario actual
            string emailUsuario = User.Identity.Name;
            var usuario = await _context.Usuarios.Where(e => e.Email == emailUsuario)
                                                 .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Se seleccionan los avisos del Empleado correspondiente al usuario actual
            var misConsultas = _context.Consultas
                            .Where(a => a.UsuarioId == usuario.Id)
                            .OrderByDescending(a => a.FechaAviso)
                            .Include(a => a.Usuario)
                            .Include(a => a.TipoConsulta);

            return View(await misConsultas.ToListAsync());

            // var mvcSoporteContexto = _context.Avisos.Include(a => a.Empleado)
            // .Include(a => a.Equipo).Include(a => a.TipoAveria)
            // var nutrimenezContexto = _context.Consultas.Include(c => c.TipoConsulta).Include(c => c.Usuario);
            // return View(await nutrimenezContexto.ToListAsync());
        }

        // GET: MisConsultas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                        .Include(c => c.TipoConsulta)
                        .Include(c => c.Usuario)
                        .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }

            // Para evitar el acceso a los avisos de otros empleados
            string emailUsuario = User.Identity.Name;
            var usuario = await _context.Usuarios
                                .Where(e => e.Email == emailUsuario)
                                .FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }
            if (consulta.UsuarioId != usuario.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(consulta);
        }

        // GET: MisConsultas/Create
        public IActionResult Create()
        {
            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion");
            //ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: MisConsultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaAviso,UsuarioId,TipoConsultaId")] Consulta consulta)
        {
            // Se asigna al aviso el Id del empleado correspondiente al usuario actual
            string emailUsuario = User.Identity.Name;
            var usuario = await _context.Usuarios
                        .Where(e => e.Email == emailUsuario)
                        .FirstOrDefaultAsync();

            if (usuario != null)
            {
                consulta.UsuarioId = usuario.Id;
            }

            if (ModelState.IsValid)
            {
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion", consulta.TipoConsultaId);
            //ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", consulta.UsuarioId);
            
            return View(consulta);
        }

        // GET: MisConsultas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            // Para evitar el acceso a los avisos de otros empleados
            string emailUsuario = User.Identity.Name;
            var usuario = await _context.Usuarios
                                .Where(e => e.Email == emailUsuario)
                                .FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }
            if (consulta.UsuarioId != usuario.Id)
            {
                return RedirectToAction(nameof(Index));
            }


            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion", consulta.TipoConsultaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", consulta.UsuarioId);
            return View(consulta);
        }

        // POST: MisConsultas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,FechaAviso,UsuarioId,TipoConsultaId")] Consulta consulta)
        {
            if (id != consulta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultaExists(consulta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion", consulta.TipoConsultaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", consulta.UsuarioId);
            return View(consulta);
        }

        // GET: MisConsultas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var consulta = await _context.Consultas
                .Include(c => c.TipoConsulta)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }

            // Para evitar el acceso a los avisos de otros empleados
            string emailUsuario = User.Identity.Name;
            var usuario = await _context.Usuarios
                                .Where(e => e.Email == emailUsuario)
                                .FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }
            if (consulta.UsuarioId != usuario.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(consulta);
        }

        // POST: MisConsultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.Id == id);
        }
    }
}
