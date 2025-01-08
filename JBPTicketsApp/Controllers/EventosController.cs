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
using Rotativa.AspNetCore;
using JBPTicketsApp.Servicios.Contrato;

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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Eventos.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
            var eventos = await _context.Eventos.ToListAsync();
            return Json(eventos);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = _context.Eventos
                .Include(e => e.Tickets) // Incluye las tickets relacionadas
                    .ThenInclude(t => t.Persona) // Cargar la persona asociada a cada ticket
                    .ThenInclude(p => p.Red)
                .Include(e => e.Tickets)
                    .ThenInclude(t => t.Categoria)
                .FirstOrDefault(e => e.IdEvento == id);

            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Redes = _context.Redes.ToList();

            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEvento,Nombre,Fecha")] Evento evento, int? totalPremium, int? precioPremium, int? totalVip, int? precioVip, int? totalGeneral, int? precioGeneral)
        {
            evento.Nombre = $"{evento.Nombre}_{evento.Fecha.Year}";
            if(_context.Eventos.Any(e => e.Nombre == evento.Nombre))
            {
                ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
                return View(evento);
            }
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

        public void CrearTickets(int idEvento, int? totalTickets, int? categoria, double? precio)
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
                    Precio = (double)precio,
                    Descuento = 0,
                    Estado = "Pendiente",
                    IdEvento = evento.IdEvento,
                    IdCategoria = (int)categoria,
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

        [HttpGet]
        public IActionResult StatusReportPdf(int? evento, string estado, int? categoria, int? red)
        {
            // Verificar que el evento está seleccionado
            if (!evento.HasValue)
            {
                ModelState.AddModelError("", "Debe seleccionar un evento.");
                ViewBag.Eventos = _context.Eventos.ToList();
                ViewBag.Redes = _context.Redes.ToList();
                return View("Index");
            }

            // Construir la consulta base
            var query = _context.Tickets
                .Include(t => t.Persona) // Incluir relación con Persona
                .ThenInclude(p => p.Red) // Incluir relación con Red
                .Include(t => t.Categoria)
                .Where(t => t.IdEvento == evento.Value) // Filtrar por evento
                .AsQueryable();

            var eventoActual = _context.Eventos.FirstOrDefault(e => e.IdEvento == evento.Value);
            var redActual = _context.Redes.FirstOrDefault(r => r.IdRed == red);
            var categoriaActual = _context.Categorias.FirstOrDefault(c => c.IdCategoria == categoria);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(t => t.Estado == estado);
            }

            if (categoria.HasValue)
            {
                query = query.Where(t => t.Categoria.IdCategoria == categoria.Value);
            }

            if (red.HasValue)
            {
                query = query.Where(t => t.Persona.Red.IdRed == red.Value);
            }

            // Calcular estadísticas
            var totalTickets = query.Count();
            var ticketsPorEstado = query.GroupBy(t => t.Estado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Count = g.Count(),
                })
                .ToList();
            var ticketsPorRed = query.GroupBy(t => t.Persona.Red.Nombre)
                                      .Select(group => new { Red = group.Key, Count = group.Count() })
                                      .ToList();
            var ticketsPorCategoria = query.GroupBy(t => t.Categoria.Nombre)
                .Select(group => new { Categoria = group.Key, Count = group.Count() })
                .ToList();



            // Ejecutar la consulta y pasar los resultados a la vista
            var ticketsFiltrados = query.ToList();

            var reportData = new
            {
                Tickets = ticketsFiltrados,
                Evento = eventoActual?.Nombre,
                Red = redActual?.Nombre,
                Categoria = categoriaActual?.Nombre,
                Estado = estado,
                //Estadísticas
                totalTickets,
                ticketsPorRed,
                ticketsPorCategoria,
                ticketsPorEstado
            };

            string pathEvento = string.IsNullOrEmpty(reportData.Evento) ? $"" : $"{reportData.Evento}";
            string pathRed = string.IsNullOrEmpty(reportData.Red) ? "" : $"_{reportData.Red}";
            string pathCategoria = string.IsNullOrEmpty(reportData.Categoria) ? "" : $"_{reportData.Categoria}_";
            string pathEstado = string.IsNullOrEmpty(reportData.Estado) ? "" : $"_{reportData.Estado}";

            string pathFileName = $"{pathEvento}{pathRed}{pathCategoria}{pathEstado}.pdf";

            return new ViewAsPdf("StatusReportPdf", reportData)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.Legal,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                FileName = pathFileName,
            };
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.IdEvento == id);
        }
    }
}
