using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;

public interface IUserService
{
    Task<Response<List<UserDto>>> ListAsync(string role,string search);
    Task<Response<UserDto>> GetUserAsync(int id);
    Task<Response<SessionDto>>AuthorizeAsync(LoginDto model);
    Task<Response<UserDto>>CreateAsync(UserDto model);
    Task<Response<bool>> UpdateAsync(UserDto model);
    Task<Response<bool>>DeleteAsync(int id);
}
public class UserService(HttpClient httpClient):IUserService
{
    public async Task<Response<List<UserDto>>> ListAsync(string role, string search)
    {
        var response = await httpClient.GetFromJsonAsync<Response<List<UserDto>>>($"User/list/{role}/{search}");
        return response;
    }

    public async Task<Response<UserDto>> GetUserAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<UserDto>>($"User/get/{id}");
        return response;
    }

    public async Task<Response<SessionDto>> AuthorizeAsync(LoginDto model)
    {
        var response = await httpClient.PostAsJsonAsync("User/authorize", model);
        Response<SessionDto>? result =await response.Content.ReadFromJsonAsync<Response<SessionDto>>();
        return result;
    }

    public async Task<Response<UserDto>> CreateAsync(UserDto model)
    {
        var response = await httpClient.PostAsJsonAsync("User/add", model);
        Response<UserDto>? result =await response.Content.ReadFromJsonAsync<Response<UserDto>>();
        return result;
    }

    public async Task<Response<bool>>UpdateAsync(UserDto model)
    {
        var response = await httpClient.PutAsJsonAsync("User/update", model);
        Response<bool>? result =await response.Content.ReadFromJsonAsync<Response<bool>>();
        return result;
    }


    public async Task<Response<bool>> DeleteAsync(int id)
    {
        var response = await httpClient.DeleteFromJsonAsync<Response<bool>>($"User/delete/{id}");
        return response;
    }
}