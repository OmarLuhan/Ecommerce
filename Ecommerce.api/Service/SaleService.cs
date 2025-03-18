using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;

namespace Ecommerce.api.Service;
public interface ISaleService
{
    Task<SaleDto> RegisterSale(SaleDto model);
}
public class SaleService(ISaleRepository saleRepository,IMapper mapper):ISaleService
{
    public async Task<SaleDto> RegisterSale(SaleDto model)
    {
        Sale sale = mapper.Map<Sale>(model);
        Sale  newSale = await saleRepository.CreateSaleAsync(sale);
        if(newSale.Id == 0)
            throw new TaskCanceledException("Failed to create sale");
        return mapper.Map<SaleDto>(newSale);
    }
}