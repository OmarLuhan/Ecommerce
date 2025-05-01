using System.Net;
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
    Task<Response<bool>> UpdateAsync(ProductDto model);
    Task<Response<bool>> DeleteAsync(int id);
}
public class ProductService(HttpClient httpClient): IProductService
{
    public async Task<Response<List<ProductDto>>?> ListAsync(string? search = null)
    {
        string query = string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}";
        var response = await httpClient.GetFromJsonAsync<Response<List<ProductDto>>>($"products/{query}");
        return response;
    }

    public async Task<Response<ProductDto>?> GetProductAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<ProductDto>>($"products/{id}");
        return response;
    }

    public Task<Response<List<ProductDto>>?> GetCatalogAsync(string category, string search)
    {
        string query = string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}";
        var response = httpClient.GetFromJsonAsync<Response<List<ProductDto>>>($"products/catalog/{category}/{query}");
        return response;
    }

    public async Task<Response<ProductDto>?> CreateAsync(ProductDto model)
    {
        var response = await httpClient.PostAsJsonAsync("products", model);
        var  result =await response.Content.ReadFromJsonAsync<Response<ProductDto>>();
        return result;
    }

    public async Task<Response<bool>> UpdateAsync(ProductDto model)
    {
        try
        {


            var response = await httpClient.PutAsJsonAsync($"$products/{model.Id}", model);
            if (response.IsSuccessStatusCode)
                return new Response<bool>
                {
                    Success = true,
                    Message = "Product updated successfully"
                };
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            return result ?? throw new HttpRequestException();
        }catch(HttpRequestException ex)
        {
            return new Response<bool>
            {
                Success = false,
                Message = $"Network error: {ex.Message}",
                Status = HttpStatusCode.ServiceUnavailable
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Success = false,
                Message = $"An error occurred: {ex.Message}",
                Status = HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"products/{id}");
            if (response.IsSuccessStatusCode)
                return new Response<bool>
                {
                    Success = true,
                    Message = "Product deleted successfully"
                };
            var result =await response.Content.ReadFromJsonAsync<Response<bool>>();
            return result ?? throw new HttpRequestException();
        }
        catch(HttpRequestException ex)
        {
            return new Response<bool>
            {
                Success = false,
                Message = $"Network error: {ex.Message}",
                Status = HttpStatusCode.ServiceUnavailable
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Success = false,
                Message = $"An error occurred: {ex.Message}",
                Status = HttpStatusCode.InternalServerError
            };
        }
    }
}