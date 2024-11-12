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
    public class RedesController : Controller
    {
        private readonly AppDbContext _context;

        public RedesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Redes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Redes.ToListAsync());
        }

        // GET: Redes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var red = await _context.Redes
                .FirstOrDefaultAsync(m => m.IdRed == id);
            if (red == null)
            {
                return NotFound();
            }

            return View(red);
        }

        // GET: Redes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Redes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRed,Nombre")] Red red)
        {
            if (ModelState.IsValid)
            {
                _context.Add(red);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(red);
        }

        // GET: Redes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var red = await _context.Redes.FindAsync(id);
            if (red == null)
            {
                return NotFound();
            }
            return View(red);
        }

        // POST: Redes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRed,Nombre")] Red red)
        {
            if (id != red.IdRed)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(red);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RedExists(red.IdRed))
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
            return View(red);
        }

        // GET: Redes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var red = await _context.Redes
                .FirstOrDefaultAsync(m => m.IdRed == id);
            if (red == null)
            {
                return NotFound();
            }

            return View(red);
        }

        // POST: Redes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var red = await _context.Redes.FindAsync(id);
            if (red != null)
            {
                _context.Redes.Remove(red);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RedExists(int id)
        {
            return _context.Redes.Any(e => e.IdRed == id);
        }
    }
}
