using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;
public interface IProductService
{
    Task<List<ProductDto>> ListAsync(string search);
    Task<List<ProductDto>> CatalogAsync(string category, string search);
    Task<ProductDto> GetProductAsync(int id);
    Task<ProductDto> CreateAsync(ProductDto model);
    Task<bool> UpdateAsync(ProductDto model);
    Task<bool> DeleteAsync(int id);
}
public class ProductService(IGenericRepository<Product> productRepository,IMapper mapper):IProductService
{
    public async Task<List<ProductDto>> ListAsync(string search)
    {
        IQueryable<Product>query= productRepository.Query(p=>p.Name.Contains(search));
        return mapper.Map<List<ProductDto>>(await query.ToListAsync());
    }

    public async Task<List<ProductDto>> CatalogAsync(string category, string search)
    {
        IQueryable<Product> query= productRepository.Query(p=>p.Name.Contains(search) &&
                                                              p.Category != null &&
                                                              p.Category.Name.Contains(category));
        return mapper.Map<List<ProductDto>>(await query.ToListAsync());
    }

    public async Task<ProductDto> GetProductAsync(int id)
    {
        IQueryable<Product>query = productRepository.Query(p=>p.Id==id);
        Product? product = await query.FirstOrDefaultAsync();
        if(product==null)
            throw new TaskCanceledException("Product not found");
        return mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(ProductDto model)
    {
        Product product= mapper.Map<Product>(model);
        Product newProduct = await productRepository.CreateAsync(product);
        if(newProduct.Id == 0)
            throw new TaskCanceledException("Failed to create product");
        return mapper.Map<ProductDto>(newProduct);
    }

    public async Task<bool> UpdateAsync(ProductDto model)
    {
        IQueryable<Product>query= productRepository.Query(p=>p.Id==model.Id);
        Product? product = await query.FirstOrDefaultAsync();
        if(product==null)
            throw new TaskCanceledException("Product not found");
        product.Name = model.Name;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        product.Description = model.Description;
        product.Image = model.Image;
        product.OfferPrice = model.OfferPrice;
        product.Stock = model.Stock;
        bool updated = await productRepository.UpdateAsync(product);
        if(!updated)
            throw new TaskCanceledException("Failed to update product");
        return updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        IQueryable<Product>query= productRepository.Query(p=>p.Id==id);
        Product? product = await query.FirstOrDefaultAsync();
        if(product==null)
            throw new TaskCanceledException("Product not found");
        bool deleted = await productRepository.DeleteAsync(product);
        if(!deleted)
            throw new TaskCanceledException("Failed to delete product");
        return deleted;
    }
}