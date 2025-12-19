using Domain.Contracts;

namespace Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetByFiltersAsync(Dictionary<string, object> filters);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
