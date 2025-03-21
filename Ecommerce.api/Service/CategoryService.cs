using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;
public interface ICategoryService
{
    Task<List<CategoryDto>> ListAsync(string search);
    Task<CategoryDto> GetCategoryAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryDto model);
    Task<bool> UpdateAsync(CategoryDto model);
    Task<bool> DeleteAsync(int id);
}
public class CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper):ICategoryService
{
    public async Task<List<CategoryDto>> ListAsync(string search)
    {
        IQueryable<Category>query= categoryRepository.Query();
        if(!string.IsNullOrEmpty(search))
            query = query.Where(c=>c.Name.Contains(search));
        return mapper.Map<List<CategoryDto>>(await query.ToListAsync());

    }

    public async Task<CategoryDto> GetCategoryAsync(int id)
    {
        IQueryable<Category> query = categoryRepository.Query(c=>c.Id==id);
        Category? category = await query.FirstOrDefaultAsync();
        if(category==null)
            throw new TaskCanceledException("Category not found");
        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto model)
    {
        Category category= mapper.Map<Category>(model);
        Category newCategory = await categoryRepository.CreateAsync(category);
        if(newCategory.Id == 0)
            throw new TaskCanceledException("Failed to create category");
        return mapper.Map<CategoryDto>(newCategory);
    }

    public async Task<bool> UpdateAsync(CategoryDto model)
    {
        IQueryable<Category> query= categoryRepository.Query(c=>c.Id==model.Id);
        Category? category = await query.FirstOrDefaultAsync();
        if(category==null)
            throw new TaskCanceledException("Category not found");
        category.Name = model.Name;
        bool updated = await categoryRepository.UpdateAsync(category);
        if (!updated) 
            throw new TaskCanceledException("Failed to update category");
        return updated;
        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        IQueryable<Category> query= categoryRepository.Query(c=>c.Id==id);
        Category? category = await query.FirstOrDefaultAsync();
        if(category==null)
            throw new TaskCanceledException("Category not found");
        bool deleted = await categoryRepository.DeleteAsync(category);
        if (!deleted) 
            throw new TaskCanceledException("Failed to delete category");
        return deleted;
    }
}