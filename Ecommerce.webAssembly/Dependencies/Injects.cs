using Blazored.LocalStorage;
using Blazored.Toast;
using Ecommerce.webAssembly.Service;

namespace Ecommerce.webAssembly.Dependencies;

public static class Injects
{
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5222/api/") });
        services.AddBlazoredLocalStorage();
        services.AddBlazoredToast();
        services.AddScoped<ICarrService, CarrService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDashboardService, DashboardService>();
    }
}