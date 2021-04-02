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
    public class TipoConsultasController : Controller
    {
        private readonly NutrimenezContexto _context;

        public TipoConsultasController(NutrimenezContexto context)
        {
            _context = context;
        }

        // GET: TipoConsultas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoConsultas.ToListAsync());
        }

        // GET: TipoConsultas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoConsulta = await _context.TipoConsultas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoConsulta == null)
            {
                return NotFound();
            }

            return View(tipoConsulta);
        }

        // GET: TipoConsultas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoConsultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] TipoConsulta tipoConsulta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoConsulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoConsulta);
        }

        // GET: TipoConsultas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoConsulta = await _context.TipoConsultas.FindAsync(id);
            if (tipoConsulta == null)
            {
                return NotFound();
            }
            return View(tipoConsulta);
        }

        // POST: TipoConsultas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] TipoConsulta tipoConsulta)
        {
            if (id != tipoConsulta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoConsulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoConsultaExists(tipoConsulta.Id))
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
            return View(tipoConsulta);
        }

        // GET: TipoConsultas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoConsulta = await _context.TipoConsultas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoConsulta == null)
            {
                return NotFound();
            }

            return View(tipoConsulta);
        }

        // POST: TipoConsultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoConsulta = await _context.TipoConsultas.FindAsync(id);
            _context.TipoConsultas.Remove(tipoConsulta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoConsultaExists(int id)
        {
            return _context.TipoConsultas.Any(e => e.Id == id);
        }
    }
}
