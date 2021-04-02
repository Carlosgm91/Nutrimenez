using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nutrimenez.Data;
using Nutrimenez.Models;

namespace Nutrimenez.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConsultasController : Controller
    {
        private readonly NutrimenezContexto _context;

        public ConsultasController(NutrimenezContexto context)
        {
            _context = context;
        }

        // GET: Consultas
        public async Task<IActionResult> Index(string strCadenaBusqueda, string busquedaActual, int? intTipoConsultaId, 
            int? tipoConsultaIdActual,int? pageNumber)
        {
            if (strCadenaBusqueda != null)
            {
                pageNumber = 1;
            }
            else
            {
                strCadenaBusqueda = busquedaActual;
            }

                ViewData["BusquedaActual"] = strCadenaBusqueda;

            if (intTipoConsultaId != null)
            {
                pageNumber = 1;
            }
            else
            {
                intTipoConsultaId = tipoConsultaIdActual;
            }
            ViewData["TipoConsultaIdActual"] = intTipoConsultaId;

            // Cargar datos de los avisos
            var consultas = _context.Consultas.AsQueryable();

            // Ordenar descendente por FechaAviso
            consultas = consultas.OrderByDescending(s => s.TipoConsulta);

            // Para buscar avisos por nombre de empleado en la lista de valores
            if (!String.IsNullOrEmpty(strCadenaBusqueda))
            {
                consultas = consultas.Where(s => s.Usuario.Nombre.Contains(strCadenaBusqueda));
            }

            //para filtrar avisos por tipo de avería
            if(intTipoConsultaId == null)
            {
                ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion");
            }
            else
            {
                ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion", intTipoConsultaId);
                consultas = consultas.Where(x => x.TipoConsultaId == intTipoConsultaId);
            }

            consultas = consultas.Include(a => a.Usuario)
                                 .Include(a => a.TipoConsulta);

            int pageSize = 5;
            return View(await PaginatedList<Consulta>.CreateAsync(consultas.AsNoTracking(),
                pageNumber ?? 1, pageSize));

            //return View(await consultas.AsNoTracking().ToListAsync());


            //var nutrimenezContexto = _context.Consultas.Include(c => c.TipoConsulta).Include(c => c.Usuario);
            //return View(await nutrimenezContexto.ToListAsync());
        }

        // GET: Consultas/Details/5
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

            return View(consulta);
        }

        // GET: Consultas/Create
        public IActionResult Create()
        {
            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Consultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaAviso,UsuarioId,TipoConsultaId")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion", consulta.TipoConsultaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", consulta.UsuarioId);
            return View(consulta);
        }

        // GET: Consultas/Edit/5
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
            ViewData["TipoConsultaId"] = new SelectList(_context.TipoConsultas, "Id", "Descripcion", consulta.TipoConsultaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", consulta.UsuarioId);
            return View(consulta);
        }

        // POST: Consultas/Edit/5
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

        // GET: Consultas/Delete/5
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

            return View(consulta);
        }

        // POST: Consultas/Delete/5
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
