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
    public class EventosController : Controller
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Eventos.ToListAsync());
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = _context.Eventos
                .Include(e => e.Tickets) // Incluye las tickets relacionadas
                    .ThenInclude(t => t.Persona) // Cargar la persona asociada a cada ticket
                .Include(e => e.Tickets)
                    .ThenInclude(t => t.Categoria)
                .FirstOrDefault(e => e.IdEvento == id);

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventos/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEvento,Nombre,Fecha")] Evento evento, int totalPremium, int precioPremium, int totalVip, int precioVip, int totalGeneral, int precioGeneral)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                var idEvento = evento.IdEvento;

                CrearTickets(idEvento, totalPremium, 1, precioPremium);
                CrearTickets(idEvento, totalVip, 2, precioVip);
                CrearTickets(idEvento, totalGeneral, 3, precioGeneral);
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
            return View(evento);
        }

        public void CrearTickets(int idEvento, int totalTickets, int categoria, double precio)
        {
            var evento = _context.Eventos.Find(idEvento);
            // Suponiendo que tienes un input con fecha y hora
            int eventYear = evento.Fecha.Year;

            var numeroInicial = _context.Tickets
                .Where(t => t.IdEvento == idEvento)
                .Count() + 1;

            for (int i = 0; i < totalTickets; i++)
            {
                var nuevoTicket = new Ticket
                {
                    Codigo = $"{evento.Nombre.Substring(0, 3).ToUpper()}_{eventYear}_{numeroInicial + i:D3}",
                    Abono = 0,
                    Precio = precio,
                    Descuento = 0,
                    Estado = "Pendiente",
                    IdEvento = evento.IdEvento,
                    IdCategoria = categoria,
                    IdPersona = 1
                };

                _context.Tickets.Add(nuevoTicket);
            }

            _context.SaveChanges();
        }

        [HttpGet]
        public IActionResult AsignarTickets(int? id)
        {
            // Busca el evento e incluye todas las tickets relacionadas
            var evento = _context.Eventos
                .Include(e => e.Tickets) // Incluye las tickets relacionadas
                    .ThenInclude(t => t.Persona) // Cargar la persona asociada a cada ticket
                .Include(e => e.Tickets)
                    .ThenInclude(t => t.Categoria)
                .FirstOrDefault(e => e.IdEvento == id);

            if (evento == null)
            {
                return NotFound();
            }

            ViewData["idPersona"] = new SelectList(_context.Personas, "IdPersona", "Nombre");

            // Retorna solo las tickets del evento en formato JSON (opcional)
            return View(evento);
        }

        [HttpPost]
        public IActionResult AsignarTickets(Dictionary<int, int> TicketsAsignados)
        {
            // Validar que el diccionario no esté vacío
            if (TicketsAsignados == null || !TicketsAsignados.Any())
            {
                return BadRequest("No se recibieron asignaciones de tickets.");
            }

            // Iterar sobre cada asignación y actualizar la persona asignada al ticket
            foreach (var asignacion in TicketsAsignados)
            {
                int ticketId = asignacion.Key;
                int personaId = asignacion.Value;

                // Recuperar el ticket de la base de datos
                var ticket = _context.Tickets.FirstOrDefault(t => t.IdTicket == ticketId);
                if (ticket != null)
                {
                    // Asignar la persona al ticket
                    ticket.IdPersona = personaId;

                    // Guardar los cambios en la base de datos
                    _context.Tickets.Update(ticket);
                }
            }

            // Guardar todos los cambios
            _context.SaveChanges();

            // Redirigir a la página que prefieras después de guardar las asignaciones
            return RedirectToAction("Index");
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEvento,Nombre,Fecha")] Evento evento)
        {
            if (id != evento.IdEvento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.IdEvento))
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
            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.IdEvento == id);
        }
    }
}
