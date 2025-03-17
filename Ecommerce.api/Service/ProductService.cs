using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;

namespace Ecommerce.api.Service;
public interface IProductService
{
    Task<List<ProductDto>> ListAsync(string search);
    Task<List<ProductDto>> ListByCategoryAsync(int categoryId, string search);
    Task<ProductDto> GetProductAsync(int id);
    Task<ProductDto> CreateAsync(ProductDto model);
    Task<bool> UpdateAsync(ProductDto model);
    Task<bool> DeleteAsync(int id);
}
public class ProductService(IGenericRepository<Product> productRepository,IMapper mapper):IProductService
{
    public Task<List<ProductDto>> ListAsync(string search)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductDto>> ListByCategoryAsync(int categoryId, string search)
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> GetProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> CreateAsync(ProductDto model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ProductDto model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}