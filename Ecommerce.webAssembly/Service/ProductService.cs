using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;

public interface IProductService
{
    Task<Response<List<ProductDto>>?> ListAsync(string search);
    Task<Response<ProductDto>?> GetProductAsync(int id);
    Task<Response<List<ProductDto>>?>GetCatalogAsync(string category, string search );
    Task<Response<ProductDto>?> CreateAsync(ProductDto model);
    Task<Response<bool>?> UpdateAsync(ProductDto model);
    Task<Response<bool>?> DeleteAsync(int id);
}
public class ProductService(HttpClient httpClient): IProductService
{
    public async Task<Response<List<ProductDto>>?> ListAsync(string? search = null)
    {
        string query = string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}";
        var response = await httpClient.GetFromJsonAsync<Response<List<ProductDto>>>($"product/list{query}");
        return response;
    }

    public async Task<Response<ProductDto>?> GetProductAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<ProductDto>>($"product/get/{id}");
        return response;
    }

    public Task<Response<List<ProductDto>>?> GetCatalogAsync(string category, string search)
    {
        string query = string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}";
        var response = httpClient.GetFromJsonAsync<Response<List<ProductDto>>>($"product/catalog/{category}/{query}");
        return response;
    }

    public async Task<Response<ProductDto>?> CreateAsync(ProductDto model)
    {
        var response = await httpClient.PostAsJsonAsync("product/add", model);
        var  result =await response.Content.ReadFromJsonAsync<Response<ProductDto>>();
        return result;
    }

    public async Task<Response<bool>?> UpdateAsync(ProductDto model)
    {
        var response = await httpClient.PutAsJsonAsync("product/update", model);
        Response<bool>? result =await response.Content.ReadFromJsonAsync<Response<bool>>();
        return result;
    }

    public async Task<Response<bool>?> DeleteAsync(int id)
    {
        var response = await httpClient.DeleteFromJsonAsync<Response<bool>>($"product/delete/{id}");
        return response;
    }
}