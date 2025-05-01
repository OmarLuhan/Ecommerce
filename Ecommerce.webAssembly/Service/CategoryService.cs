using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;
public interface ICategoryService
{
    Task<Response<List<CategoryDto>>?> ListAsync(string? search=null);
    Task<Response<CategoryDto>?> GetCategoryAsync(int id);
    Task<Response<CategoryDto>?> CreateAsync(CategoryDto model);
    Task<Response<bool>?> UpdateAsync(CategoryDto model);
    Task<Response<bool>?> DeleteAsync(int id);
}
public class CategoryService(HttpClient httpClient):ICategoryService
{
    public async Task<Response<List<CategoryDto>>?> ListAsync(string? search = null)
    {
        string query = string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}";
        var response = await httpClient.GetFromJsonAsync<Response<List<CategoryDto>>>($"categories{query}");
        return response;
    }



    public async Task<Response<CategoryDto>?> GetCategoryAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<CategoryDto>>($"categories/{id}");
        return response;
    }

    public async Task<Response<CategoryDto>?> CreateAsync(CategoryDto model)
    {
        var response = await httpClient.PostAsJsonAsync("categories", model);
        Response<CategoryDto>? result =await response.Content.ReadFromJsonAsync<Response<CategoryDto>>();
        return result;
    }

    public async Task<Response<bool>?> UpdateAsync(CategoryDto model)
    {
        var response = await httpClient.PutAsJsonAsync("categories", model);
        Response<bool>? result =await response.Content.ReadFromJsonAsync<Response<bool>>();
        return result;
    }

    public async Task<Response<bool>?> DeleteAsync(int id)
    {
        var response = await httpClient.DeleteFromJsonAsync<Response<bool>>($"categories/{id}");
        return response;
    }
}