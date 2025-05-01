using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;
public interface IProductService
{
    Task<PageList<ProductDto>> ListAsync(SpecParam? specParam,string search);
    Task<PageList<ProductDto>> CatalogAsync(SpecParam? specParam,string category, string search);
    Task<ProductDto> GetProductAsync(int id);
    Task<ProductDto> CreateAsync(ProductCreateDto model);
    Task UpdateAsync(ProductUpdateDto model);
    Task DeleteAsync(int id);
}
public class ProductService(IProductRepository productRepository,IMapper mapper):IProductService
{
    public async Task<PageList<ProductDto>> ListAsync(SpecParam? specParam, string search)
    {
        ArgumentNullException.ThrowIfNull(specParam);
        
        IQueryable<Product>query= productRepository.Query();
        if(!string.IsNullOrEmpty(search))
            query = query.Where(p=>p.Name.Contains(search));
        
        if(!string.IsNullOrEmpty(specParam.SortBy))
        {
            query = specParam.SortDesc
                ? query.OrderByDescending(p => EF.Property<object>(p, specParam.SortBy))
                : query.OrderBy(p => EF.Property<object>(p, specParam.SortBy));
        }
        var queryDto = query.ProjectTo<ProductDto>(mapper.ConfigurationProvider);
        return await PageList<ProductDto>.ToPageList(
            queryDto,
            specParam.PageNumber,
            specParam.PageSize
        );
    }

    public async Task<PageList<ProductDto>> CatalogAsync(SpecParam? specParam,string category, string search)
    {
        ArgumentNullException.ThrowIfNull(specParam);
        if(category=="all")
            category="";
        IQueryable<Product> query = productRepository.Query().Include(c=>c.Category);
        if(!string.IsNullOrEmpty(category))
            query = query.Where(c=>c.Category!.Name==category);
        if(!string.IsNullOrEmpty(search))
            query = query.Where(s=>s.Name.Contains(search));
        if(!string.IsNullOrEmpty(specParam.SortBy))
        {
            query = specParam.SortDesc
                ? query.OrderByDescending(p => EF.Property<object>(p, specParam.SortBy))
                : query.OrderBy(p => EF.Property<object>(p, specParam.SortBy));
        }
        var queryDto = query.ProjectTo<ProductDto>(mapper.ConfigurationProvider);
        return await PageList<ProductDto>.ToPageList(
            queryDto,
            specParam.PageNumber,
            specParam.PageSize
        );
    }

    public async Task<ProductDto> GetProductAsync(int id)
    {
        Product? product =await  productRepository.GetAsync(p=>p.Id==id);
        if(product==null)
            throw new TaskCanceledException("Product not found");
        return mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(ProductCreateDto model)
    {
        Product product= mapper.Map<Product>(model);
        Product newProduct = await productRepository.CreateProduct(product);
        if(newProduct.Id == 0)
            throw new TaskCanceledException("Failed to create product");
        return mapper.Map<ProductDto>(newProduct);
    }

    public async Task  UpdateAsync(ProductUpdateDto model)
    {
        Product? product= await productRepository.GetAsync(p=>p.Id==model.Id,track:true);
        if(product==null)
            throw new TaskCanceledException("Product not found");
        product.Name = model.Name;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        product.Description = model.Description;
        product.Image = model.Image;
        product.OfferPrice = model.OfferPrice;
        product.Stock = model.Stock;
        await productRepository.UpdateAsync(product);
    }

    public async Task  DeleteAsync(int id)
    {
        if (id == 0)
            throw new ValidationException(nameof(id) + " cannot be 0");
        await productRepository.DeleteAsync(id);
       
    }
}