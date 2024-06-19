namespace Persistence.Repository.Interfaces;

public interface IRepositoryBase <T> where T : class
{
    Task CreateAsync(T entity);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
