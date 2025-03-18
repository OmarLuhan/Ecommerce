using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;

namespace Ecommerce.api.Service;
public interface ISaleService
{
    Task<SaleDto> RegisterSaleAsync(SaleDto model);
    Task<SaleDto> GetSaleAsync(int id);
}
public class SaleService(ISaleRepository saleRepository,IMapper mapper):ISaleService
{
    public async Task<SaleDto> RegisterSaleAsync(SaleDto model)
    {
        Sale sale = mapper.Map<Sale>(model);
        Sale  newSale = await saleRepository.CreateSaleAsync(sale);
        if(newSale.Id == 0)
            throw new TaskCanceledException("Failed to create sale");
        return mapper.Map<SaleDto>(newSale);
    }

    public async Task<SaleDto> GetSaleAsync(int id)
    {
        Sale? sale = await saleRepository.GetAsync(s=>s.Id == id);
        if(sale == null)
            throw new TaskCanceledException("Sale not found");
        return mapper.Map<SaleDto>(sale);
    }
}