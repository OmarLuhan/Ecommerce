using System.Linq.Expressions;
using Ecommerce.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Repository;
public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query(Expression<Func<T, bool>>? filters = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filters, bool tracked = true);
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

    public IQueryable<T> Query(Expression<Func<T, bool>>? filters = null)
    {
        IQueryable<T> query = filters == null ? _dbSet : _dbSet.Where(filters);
        return query;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filters, bool tracked = true)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked) query = query.AsNoTracking();
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