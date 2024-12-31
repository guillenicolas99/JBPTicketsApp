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
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Tickets.Include(t => t.Categoria).Include(t => t.Evento).Include(t => t.Persona);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Evento)
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(m => m.IdTicket == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "Nombre");
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "Nombre");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTicket,Codigo,Abono,Precio,Descuento,FechaDescuento,Estado,IdEvento,IdCategoria,IdPersona")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", ticket.IdCategoria);
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "Nombre", ticket.IdEvento);
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "Nombre", ticket.IdPersona);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", ticket.IdCategoria);
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "Nombre", ticket.IdEvento);
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "Nombre", ticket.IdPersona);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTicket,Codigo,Abono,Precio,Descuento,FechaDescuento,Estado,IdEvento,IdCategoria,IdPersona")] Ticket ticket)
        {
            if (id != ticket.IdTicket)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.IdTicket))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", ticket.IdCategoria);
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "Nombre", ticket.IdEvento);
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "Nombre", ticket.IdPersona);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Evento)
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(m => m.IdTicket == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.IdTicket == id);
        }

        //MÉTODO PARA ABONAR A UNA TICKET
        [HttpPost]
        public IActionResult Abonar([FromBody] AbonarRequest request)
        {
            if (string.IsNullOrEmpty(request.CodigoTicket) || request.MontoAbono <= 0)
            {
                return BadRequest(new { message = "Datos inválidos" });
            }

            // Lógica para registrar el abono en la base de datos
            var ticket = _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Evento)
                .Include(t => t.Persona)
                .FirstOrDefault(t => t.Codigo == request.CodigoTicket);
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket no encontrado" });
            }

            if (ticket.Estado == "Pagado")
            {
                return BadRequest(new { message = "El ticket ya está pagado." });
            }

            if (ticket.Persona?.Nombre == "Sin asignar")
            {
                return BadRequest(new { message = "El ticket debe estar asignado." });
            }

            ticket.Abono += request.MontoAbono;
            ticket.Estado = "Abonado";

            if (ticket.Abono > ticket.Precio)
            {
                return BadRequest(new { message = "El abono supera el saldo pendiente." });
            }

            if (ticket.Abono == ticket.Precio)
            {
                ticket.Estado = "Pagado";
            }

            _context.SaveChanges();

            return Ok(new { message = "Abono registrado correctamente." });
        }

        public class AbonarRequest
        {
            public string CodigoTicket { get; set; }
            public double MontoAbono { get; set; }
        }


        //MÉTODO PARA CANCELAR/PAGAR UNA TICKET
        [HttpPost]
        public IActionResult CancelarTicket(int ticketId)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket == null || ticket.Estado == "Pagado")
            {
                return NotFound("La ticket no puede ser cancelada.");
            }

            ticket.Estado = "Cancelado";
            _context.SaveChanges();

            return RedirectToAction("ReporteTicketsPendientes", new { eventoId = ticket.IdEvento });
        }


    }
}
