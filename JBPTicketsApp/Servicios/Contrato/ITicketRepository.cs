using JBPTicketsApp.Models.Entities;

namespace JBPTicketsApp.Servicios.Contrato
{
    public interface ITicketRepository : IRepositoryAsync<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketsByEventAsync(int eventId);
        Task<IEnumerable<Ticket>> GetPendingTicketsAsync();

    }
}
