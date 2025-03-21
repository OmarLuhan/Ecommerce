using Ecommerce.api.Helpers;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Ecommerce.api.Service;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Dependencies;

public static class Injects
{
    public static void Inject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DbEcommerceContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),new MySqlServerVersion(new Version(8, 0, 21))));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<ICategoryService,CategoryService>();
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:5036","https://localhost:7067");
        }));
    }
}