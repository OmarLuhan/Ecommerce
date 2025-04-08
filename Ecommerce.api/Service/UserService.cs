using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;

public interface IUserService
{
    Task<PageList<UserDto>> ListAsync(SpecParam? specParams,string role,string search);
    Task<UserDto> GetUserAsync(int id);
    Task<SessionDto>AuthorizeAsync(LoginDto model);
    Task<UserDto>CreateAsync(UserDto model);
    Task<bool>UpdateAsync(UserDto model);
    Task<bool>DeleteAsync(int id);
}
public class UserService(IGenericRepository<User> userRepository,IMapper mapper):IUserService
{
    public async Task<PageList<UserDto>> ListAsync(SpecParam? specParams, string role, string search)
    {
   
        ArgumentNullException.ThrowIfNull(specParams);

        if (specParams.PageSize <= 0 || specParams.PageNumber <= 0)
            throw new ArgumentException("Parámetros de paginación inválidos");
        
        IQueryable<User> query = userRepository.Query(track: false);


        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role == role);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));
        }
        
        if (!string.IsNullOrEmpty(specParams.SortBy))
        {
            query = specParams.SortDesc
                ? query.OrderByDescending(u => EF.Property<object>(u, specParams.SortBy))
                : query.OrderBy(u => EF.Property<object>(u, specParams.SortBy));
        }

        var queryDto = query.ProjectTo<UserDto>(mapper.ConfigurationProvider);
        
        return await PageList<UserDto>.ToPageList(
            queryDto,
            specParams.PageNumber,
            specParams.PageSize
        );
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        IQueryable<User> query= userRepository.Query(u=>u.Id==id);
        User? user = await query.FirstOrDefaultAsync();
        if(user==null)
            throw new TaskCanceledException("User not found");
        return mapper.Map<UserDto>(user);
    }

    public async Task<SessionDto> AuthorizeAsync(LoginDto model)
    {
        IQueryable<User> query= userRepository.Query(u=>u.Email==model.Email && u.Password==model.Password);
        User? user = await query.FirstOrDefaultAsync();
        if(user==null)
            throw new TaskCanceledException("Invalid email or password");
        return mapper.Map<SessionDto>(user);
    }

    public async Task<UserDto> CreateAsync(UserDto model)
    {
       User user = mapper.Map<User>(model);
       User newUser = await userRepository.CreateAsync(user);
       if(newUser.Id == 0)
            throw new TaskCanceledException("Failed to create user");
       return mapper.Map<UserDto>(newUser);
    }

    public async Task<bool> UpdateAsync(UserDto model)
    {
        IQueryable<User> query = userRepository.Query(u=>u.Id==model.Id);
        User? user = await query.FirstOrDefaultAsync();
        if(user==null)
            throw new TaskCanceledException("User not found");
        user.FullName=model.FullName;
        user.Email=model.Email;
        user.Password=model.Password;
        bool updated = await userRepository.UpdateAsync(user);
        if(!updated)
            throw new TaskCanceledException("Failed to update user");
        return updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        IQueryable<User> query = userRepository.Query(u=>u.Id==id);
        User? user = await query.FirstOrDefaultAsync();
        if(user==null)
            throw new TaskCanceledException("User not found");
        bool deleted = await userRepository.DeleteAsync(user);
        if(!deleted)
            throw new TaskCanceledException("Failed to delete user");
        return deleted;
    }
}