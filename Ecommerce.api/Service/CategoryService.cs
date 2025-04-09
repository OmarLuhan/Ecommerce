using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;
public interface ICategoryService
{
    Task<PageList<CategoryDto>> ListAsync(SpecParam? specParam,string  search);
    Task<CategoryDto> GetCategoryAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryCreateDto model);
    Task UpdateAsync(CategoryUpdateDto model);
    Task DeleteAsync(int id);
}
public class CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper):ICategoryService
{
    public async Task<PageList<CategoryDto>> ListAsync(SpecParam? specParams,string search)
    {
        ArgumentNullException.ThrowIfNull(specParams);
        IQueryable<Category>query= categoryRepository.Query();
        if (!string.IsNullOrEmpty(search))
        {
            search = search.Trim();;
            query = query.Where(c => c.Name.Contains(search));
        }
        if (!string.IsNullOrEmpty(specParams.SortBy))
        {
            query = specParams.SortDesc
                ? query.OrderByDescending(u => EF.Property<object>(u, specParams.SortBy))
                : query.OrderBy(u => EF.Property<object>(u, specParams.SortBy));
        }
        IQueryable<CategoryDto>queryDto = query.ProjectTo<CategoryDto>(mapper.ConfigurationProvider);
        return await PageList<CategoryDto>.ToPageList(
            queryDto,
            specParams.PageNumber,
            specParams.PageSize
        );
    }

    public async Task<CategoryDto> GetCategoryAsync(int id)
    {
        Category?category = await categoryRepository.GetAsync(c=>c.Id==id);
        if(category==null)
            throw new TaskCanceledException("Category not found");
        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto model)
    {
        Category category= mapper.Map<Category>(model);
        Category newCategory = await categoryRepository.CreateAsync(category);
        if(newCategory.Id == 0)
            throw new TaskCanceledException("Failed to create category");
        return mapper.Map<CategoryDto>(newCategory);
    }

    public async Task UpdateAsync(CategoryUpdateDto model)
    {
        IQueryable<Category> query= categoryRepository.Query(c=>c.Id==model.Id);
        Category? category = await query.FirstOrDefaultAsync();
        if(category==null)
            throw new TaskCanceledException("Category not found");
        category.Name = model.Name;
        await categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        await categoryRepository.DeleteAsync(id);
   
    }
}