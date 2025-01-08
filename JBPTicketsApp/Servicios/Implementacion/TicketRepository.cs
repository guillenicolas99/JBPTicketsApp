using JBPTicketsApp.Models.Entities;
using JBPTicketsApp.Servicios.Contrato;
using Microsoft.EntityFrameworkCore;

namespace JBPTicketsApp.Servicios.Implementacion
{
    public class TicketRepository : RepositoryAsync<Ticket>, ITicketRepository
    {
        public TicketRepository(DbContext context) : base(context) { }

        public async Task<IEnumerable<Ticket>> GetTicketsByEventAsync(int eventId)
        {
            return await _dbSet.Where(t => t.IdEvento == eventId).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetPendingTicketsAsync()
        {
            return await _dbSet.Where(t => t.Estado == "Pendiente").ToListAsync();
        }

    }
}
