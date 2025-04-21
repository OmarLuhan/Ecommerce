using System.Data;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Ecommerce.api.Repository;
public interface IProductRepository:IGenericRepository<Product>
{
    Task<Product>CreateProduct(Product product);
}
public class ProductRepository(DbEcommerceContext context) : GenericRepository<Product>(context), IProductRepository
{
    private readonly DbEcommerceContext _context = context;
    public async Task<Product> CreateProduct(Product product)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        object[] parameters =
        [
            new MySqlParameter("@p0", product.Name),
            new MySqlParameter("@p1", product.Description),
            new MySqlParameter("@p2", product.CategoryId),
            new MySqlParameter("@p3", product.Price),
            new MySqlParameter("@p4", product.OfferPrice),
            new MySqlParameter("@p5", product.Stock),
            new MySqlParameter("@p6", product.Image)
        ];
        string commandText = "CALL sp_create_product(@p0, @p1, @p2, @p3, @p4, @p5, @p6)";
        int rows=await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
        if(rows==0) throw new TaskCanceledException("0 rows affected");
        Product? idOnly = await _context.Set<Product>()
            .FromSqlRaw("SELECT * FROM Product WHERE Id = LAST_INSERT_ID()")
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (idOnly== null)
            throw new TaskCanceledException("Product not created");
        product.Id = idOnly.Id;
        await transaction.CommitAsync();
        return product;
    }
}
