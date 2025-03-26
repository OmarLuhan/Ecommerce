using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;

public interface IUserService
{
    Task<Response<List<UserDto>>?> ListAsync(string role,string search);
    Task<Response<UserDto>?> GetUserAsync(int id);
    Task<Response<SessionDto>?> AuthorizeAsync(LoginDto model);
    Task<Response<UserDto>?> CreateAsync(UserDto model);
    Task<Response<bool>?> UpdateAsync(UserDto model);
    Task<Response<bool>?> DeleteAsync(int id);
}
public class UserService(HttpClient httpClient):IUserService
{
    public async Task<Response<List<UserDto>>?> ListAsync(string role, string search)
    {
        string query = string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}";
        var response = await httpClient.GetFromJsonAsync<Response<List<UserDto>>>($"user/list/{role}/{query}");
        return response;
    }

    public async Task<Response<UserDto>?> GetUserAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<UserDto>>($"user/get/{id}");
        return response;
    }

    public async Task<Response<SessionDto>?> AuthorizeAsync(LoginDto model)
    {
        var response = await httpClient.PostAsJsonAsync("user/authorize", model);
        Response<SessionDto>? result =await response.Content.ReadFromJsonAsync<Response<SessionDto>>();
        return result;
    }

    public async Task<Response<UserDto>?> CreateAsync(UserDto model)
    {
        var response = await httpClient.PostAsJsonAsync("user/add", model);
        Response<UserDto>? result =await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        return result;
    }

    public async Task<Response<bool>?> UpdateAsync(UserDto model)
    {
        var response = await httpClient.PutAsJsonAsync("user/update", model);
        Response<bool>? result =await response.Content.ReadFromJsonAsync<Response<bool>>();
        return result;
    }


    public async Task<Response<bool>?> DeleteAsync(int id)
    {
        var response = await httpClient.DeleteFromJsonAsync<Response<bool>>($"user/delete/{id}");
        return response;
    }
}