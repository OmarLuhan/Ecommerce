using Ecommerce.api.Helpers;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Dependencies;

public static class Injects
{
    public static void Inject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbEcommerceContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),new MySqlServerVersion(new Version(8, 0, 41))));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }
}