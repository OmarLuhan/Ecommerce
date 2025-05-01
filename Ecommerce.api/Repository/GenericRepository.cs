using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Ecommerce.api.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Repository;
public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Query(Expression<Func<T, bool>>? filters = null,bool track = false);
    Task<T?> GetAsync(Expression<Func<T, bool>> filters, bool track= false);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
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
        IQueryable<T>query = track ? _dbSet : _dbSet.AsNoTracking();
        return filters != null ? query.Where(filters) : query;
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


    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if(entity== null)
            throw new ValidationException($"Entity with id {id} not found");
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
   
}