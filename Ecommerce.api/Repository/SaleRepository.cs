using Ecommerce.api.Model;

namespace Ecommerce.api.Repository;
public interface ISaleRepository:IGenericRepository<Sale>
{
    Task<Sale> CreateSaleAsync(Sale sale);
}

public class SaleRepository(DbEcommerceContext context) : GenericRepository<Sale>(context), ISaleRepository
{
    private readonly DbEcommerceContext _context = context;

    public async Task<Sale> CreateSaleAsync(Sale sale)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (!await SubtractStock(sale.SaleDetails))
            {
                throw new Exception("Product not found in detail");
            }
            var newSale = await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return newSale.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<bool> SubtractStock(IEnumerable<SaleDetail> details)
    {
        foreach (var detail in details)
        {
            var product = await _context.Products.FindAsync(detail.ProductId);
            if (product == null)
            {
                return false;
            }
            product.Stock -= detail.Quantity;
        }
        return true;
    }
}
