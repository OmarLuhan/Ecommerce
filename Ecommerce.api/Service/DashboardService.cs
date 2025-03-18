using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;
public interface IDashboardService
{
    Task<DashboardDto>SummaryAsync();
}
public class DashboardService(IGenericRepository<User> userRepository, 
                              IGenericRepository<Product>productRepository,
                              ISaleRepository saleRepository):IDashboardService
{
    private async Task<string >Income()
    {
        IQueryable<Sale> query = saleRepository.Query();
        decimal income = await query.SumAsync(s=>s.Total);
        return income.ToString("C");
    }
    private async Task<int> CountSales()
    {
        IQueryable<Sale> query = saleRepository.Query();
        int count = await query.CountAsync();
        return count;
    }
    private async Task<int>CountCustomers()
    {
        IQueryable<User> query = userRepository.Query(u=>u.Role=="customer");
        int count = await query.CountAsync();
        return count;
    }
    private async Task<int>CountProducts()
    {
        IQueryable<Product> query = productRepository.Query();
        int count = await query.CountAsync();
        return count;
    }
    public async Task<DashboardDto> SummaryAsync()
    {
        DashboardDto dto = new()
        {
            Income = await Income(),
            Sales = await CountSales(),
            Customers = await CountCustomers(),
            Products = await CountProducts()
        };
        return dto;
    }
}