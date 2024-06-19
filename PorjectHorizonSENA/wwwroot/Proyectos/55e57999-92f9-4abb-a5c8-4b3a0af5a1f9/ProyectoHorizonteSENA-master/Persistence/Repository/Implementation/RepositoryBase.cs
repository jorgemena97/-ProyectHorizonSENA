using Microsoft.EntityFrameworkCore;
using Persistence.Repository.Interfaces;

namespace Persistence.Repository.Implementation;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly HorizonteDBContext _context;

    public RepositoryBase(HorizonteDBContext context)
    {
        _context = context;

    }
    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public Task<T> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
