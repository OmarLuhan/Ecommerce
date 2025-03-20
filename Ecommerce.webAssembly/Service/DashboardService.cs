using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;
public interface IDashboardService
{
    Task<Response<DashboardDto>>SummaryAsync();
}
public class DashboardService(HttpClient httpClient):IDashboardService
{
    public Task<Response<DashboardDto>> SummaryAsync()
    {
        var response = httpClient.GetFromJsonAsync<Response<DashboardDto>>("dashboard/summary");
        return response;
    }
}