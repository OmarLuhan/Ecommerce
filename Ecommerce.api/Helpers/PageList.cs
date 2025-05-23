using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Helpers;

public class PageList<T>(List<T> items, int count, int pageNumber, int pageSize)
    : IReadOnlyList<T>
{
    public MetaData MetaData { get; } = new()
    {
        CurrentPage = pageNumber,
        PageSize = pageSize,
        TotalCount = count,
        TotalPages = (int)Math.Ceiling(count / (double)pageSize)
    };
    
    public T this[int index] => items[index];
    public int Count => items.Count;
    public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static async Task<PageList<T>> ToPageList(
        IQueryable<T> query, 
        int pageNumber, 
        int pageSize)
    {
        var count = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PageList<T>(items, count, pageNumber, pageSize);
    }
}