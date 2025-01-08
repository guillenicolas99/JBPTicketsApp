using System.Linq.Expressions;

namespace JBPTicketsApp.Servicios.Contrato
{
    public interface IRepositoryAsync <T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int? id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(int id);

        //Task<T> FindFilter(Expression<Func<T, bool>> expr);
    }
}
