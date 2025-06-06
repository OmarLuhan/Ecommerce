using System.Net.Http.Json;
using Ecommerce.webAssembly.Dto;
using Ecommerce.webAssembly.Helpers;

namespace Ecommerce.webAssembly.Service;
public interface IDashboardService
{
    Task<Response<DashboardDto>?>SummaryAsync();
}
public class DashboardService(HttpClient httpClient):IDashboardService
{
    public Task<Response<DashboardDto>?> SummaryAsync()
    {
        Task<Response<DashboardDto>?> response = httpClient.GetFromJsonAsync<Response<DashboardDto>>("dashboards/summary");
        return response;
    }
}