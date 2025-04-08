using System.Linq.Expressions;
using Ecommerce.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Repository;
public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query(Expression<Func<T, bool>>? filters = null,bool track = false);
    Task<T?> GetAsync(Expression<Func<T, bool>> filters, bool track= false);
    Task<T> CreateAsync(T entity);
    Task<bool>UpdateAsync(T entity);
    Task <bool>DeleteAsync(T entity);
}
public class GenericRepository <T> : IGenericRepository<T> where T : class
{
    private readonly DbEcommerceContext _context;
    private readonly DbSet<T> _dbSet;
    public GenericRepository(DbEcommerceContext context)
    {
        _context=context;
        _dbSet=_context.Set<T>();
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? filters = null, bool track = false)
    {
        var query = filters == null ? _dbSet : _dbSet.Where(filters);
        return track ? query : query.AsNoTracking();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filters, bool track = false)
    {
        var query = track ? _dbSet : _dbSet.AsNoTracking();
        return await query.Where(filters).FirstOrDefaultAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
   
}