using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;

public interface IUserService
{
    Task<PageList<UserDto>> ListAsync(SpecParam? specParams,string role,string search);
    Task<UserDto> GetUserAsync(int id);
    Task<SessionDto>AuthorizeAsync(LoginDto model);
    Task<UserDto>CreateAsync(UserCreateDto model);
    Task UpdateAsync(UserUpdateDto model);
    Task DeleteAsync(int id);
}
public class UserService(IGenericRepository<User> userRepository,IMapper mapper):IUserService
{
    public async Task<PageList<UserDto>> ListAsync(SpecParam? specParams, string role, string search)
    {
   
        ArgumentNullException.ThrowIfNull(specParams);
        // var propertyExists = specParams.SortBy != null && typeof(User).GetProperty(specParams.SortBy) != null;
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
        User? user = await userRepository.GetAsync(u=>u.Id==id);
        
        return user==null?
            throw new TaskCanceledException("User not found"):
        mapper.Map<UserDto>(user);
    }

    public async Task<SessionDto> AuthorizeAsync(LoginDto model)
    {
        User? user= await userRepository.GetAsync(u=>u.Email==model.Email && u.Password==model.Password);
        if(user==null)
            throw new TaskCanceledException("Invalid email or password");
        return mapper.Map<SessionDto>(user);
    }

    public async Task<UserDto> CreateAsync(UserCreateDto model)
    {
       User user = mapper.Map<User>(model);
       User newUser = await userRepository.CreateAsync(user);
       if(newUser.Id == 0)
            throw new TaskCanceledException("Failed to create user");
       return mapper.Map<UserDto>(newUser);
    }

    public async Task UpdateAsync(UserUpdateDto model)
    {
        User? user =await  userRepository.GetAsync(u=>u.Id==model.Id,track:true);
        if(user==null)
            throw new TaskCanceledException("User not found");
        user.FullName=model.FullName;
        user.Email=model.Email;
        user.Password=model.Password;
        await userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        await userRepository.DeleteAsync(id);
    }
}