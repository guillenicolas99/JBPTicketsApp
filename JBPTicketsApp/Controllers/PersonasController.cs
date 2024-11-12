using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JBPTicketsApp.Models;
using JBPTicketsApp.Models.Entities;
using JBPTicketsApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace JBPTicketsApp.Controllers
{
    [Authorize]
    public class PersonasController : Controller
    {
        private readonly AppDbContext _context;

        public PersonasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Personas.Include(p => p.NivelLiderazgo).Include(p => p.Red);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.NivelLiderazgo)
                .Include(p => p.Red)
                .FirstOrDefaultAsync(m => m.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            ViewData["IdNivelLiderazgo"] = new SelectList(_context.NivelesLiderazgo, "IdNivelLiderazgo", "Nombre");
            ViewData["IdRed"] = new SelectList(_context.Redes, "IdRed", "Nombre");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,Nombre,IdRed,IdNivelLiderazgo")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNivelLiderazgo"] = new SelectList(_context.NivelesLiderazgo, "IdNivelLiderazgo", "Nombre", persona.IdNivelLiderazgo);
            ViewData["IdRed"] = new SelectList(_context.Redes, "IdRed", "Nombre", persona.IdRed);
            return View(persona);
        }*/

        [HttpPost]
        public IActionResult Create(RegistrarPersonaViewModel model, Persona persona)
        {
            try
            {
                foreach (var oP in model.Persona)
                {
                    Persona oPersona = new Persona();
                    oPersona.Nombre = oP.Nombre;
                    oPersona.IdRed = oP.IdRed;
                    oPersona.IdNivelLiderazgo = oP.IdTitulo;
                    _context.Personas.Add(oPersona);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }catch (Exception ex)
            {
                ViewBag.message = "No se pudieron crear los registros";
                ViewData["IdNivelLiderazgo"] = new SelectList(_context.NivelesLiderazgo, "IdNivelLiderazgo", "Nombre", persona.IdNivelLiderazgo);
                ViewData["IdRed"] = new SelectList(_context.Redes, "IdRed", "Nombre", persona.IdRed);
                return View(model);
            }
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            ViewData["IdNivelLiderazgo"] = new SelectList(_context.NivelesLiderazgo, "IdNivelLiderazgo", "Nombre", persona.IdNivelLiderazgo);
            ViewData["IdRed"] = new SelectList(_context.Redes, "IdRed", "Nombre", persona.IdRed);
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,Nombre,IdRed,IdNivelLiderazgo")] Persona persona)
        {
            if (id != persona.IdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.IdPersona))
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
            ViewData["IdNivelLiderazgo"] = new SelectList(_context.NivelesLiderazgo, "IdNivelLiderazgo", "Nombre", persona.IdNivelLiderazgo);
            ViewData["IdRed"] = new SelectList(_context.Redes, "IdRed", "Nombre", persona.IdRed);
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.NivelLiderazgo)
                .Include(p => p.Red)
                .FirstOrDefaultAsync(m => m.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.IdPersona == id);
        }
    }
}
