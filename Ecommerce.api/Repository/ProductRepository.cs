using System.Data;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "sp_create_product";
        command.CommandType = CommandType.StoredProcedure;
    
        command.Parameters.Add(new MySqlParameter("Name", product.Name));
        command.Parameters.Add(new MySqlParameter("Description", product.Description));
        command.Parameters.Add(new MySqlParameter("CategoryId", product.CategoryId));
        command.Parameters.Add(new MySqlParameter("Price", product.Price));
        command.Parameters.Add(new MySqlParameter("OfferPrice", product.OfferPrice));
        command.Parameters.Add(new MySqlParameter("Stock", product.Stock));
        command.Parameters.Add(new MySqlParameter("Image", product.Image));
        
        var outputParam = new MySqlParameter
        {
            ParameterName = "Id",
            MySqlDbType = MySqlDbType.Int32,
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParam);
        int rows=await command.ExecuteNonQueryAsync();
        if (rows == 0)
            throw new TaskCanceledException("0 rows affected");
        product.Id = Convert.ToInt32(outputParam.Value);
        return product;
    }
}
