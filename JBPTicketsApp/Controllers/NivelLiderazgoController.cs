using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JBPTicketsApp.Models;
using JBPTicketsApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace JBPTicketsApp.Controllers
{
    [Authorize]
    public class NivelLiderazgoController : Controller
    {
        private readonly AppDbContext _context;

        public NivelLiderazgoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: NivelLiderazgo
        public async Task<IActionResult> Index()
        {
            return View(await _context.NivelesLiderazgo.ToListAsync());
        }

        // GET: NivelLiderazgo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelLiderazgo = await _context.NivelesLiderazgo
                .FirstOrDefaultAsync(m => m.IdNivelLiderazgo == id);
            if (nivelLiderazgo == null)
            {
                return NotFound();
            }

            return View(nivelLiderazgo);
        }

        // GET: NivelLiderazgo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NivelLiderazgo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNivelLiderazgo,Nombre")] NivelLiderazgo nivelLiderazgo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nivelLiderazgo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nivelLiderazgo);
        }

        // GET: NivelLiderazgo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelLiderazgo = await _context.NivelesLiderazgo.FindAsync(id);
            if (nivelLiderazgo == null)
            {
                return NotFound();
            }
            return View(nivelLiderazgo);
        }

        // POST: NivelLiderazgo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNivelLiderazgo,Nombre")] NivelLiderazgo nivelLiderazgo)
        {
            if (id != nivelLiderazgo.IdNivelLiderazgo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nivelLiderazgo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NivelLiderazgoExists(nivelLiderazgo.IdNivelLiderazgo))
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
            return View(nivelLiderazgo);
        }

        // GET: NivelLiderazgo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelLiderazgo = await _context.NivelesLiderazgo
                .FirstOrDefaultAsync(m => m.IdNivelLiderazgo == id);
            if (nivelLiderazgo == null)
            {
                return NotFound();
            }

            return View(nivelLiderazgo);
        }

        // POST: NivelLiderazgo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nivelLiderazgo = await _context.NivelesLiderazgo.FindAsync(id);
            if (nivelLiderazgo != null)
            {
                _context.NivelesLiderazgo.Remove(nivelLiderazgo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NivelLiderazgoExists(int id)
        {
            return _context.NivelesLiderazgo.Any(e => e.IdNivelLiderazgo == id);
        }
    }
}
