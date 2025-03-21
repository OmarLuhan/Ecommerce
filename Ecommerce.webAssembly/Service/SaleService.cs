using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;
public interface ISaleService
{
  
    Task<Response<SaleDto>?> SaveSaleAsync(SaleDto sale);
    Task<Response<SaleDto>?> GetSaleAsync(int id);
}
public class SaleService(HttpClient httpClient): ISaleService
{
   
    public async Task<Response<SaleDto>?> SaveSaleAsync(SaleDto sale)
    {
        var response = await httpClient.PostAsJsonAsync("sale/add", sale);
        var result =await response.Content.ReadFromJsonAsync<Response<SaleDto>>();
        return result;
    }

    public async Task<Response<SaleDto>?> GetSaleAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<SaleDto>>($"sale/get/{id}");
        return response;
    }
}
