using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;

public interface IUserService
{
    Task<List<UserDto>> ListAsync(string role,string search);
    Task<UserDto> GetUserAsync(int id);
    Task<SessionDto>AuthorizeAsync(LoginDto model);
    Task<UserDto>CreateAsync(UserDto model);
    Task<bool>UpdateAsync(UserDto model);
    Task<bool>DeleteAsync(int id);
}
public class UserService(IGenericRepository<User> userRepository,IMapper mapper):IUserService
{
    public Task<List<UserDto>> ListAsync(string role, string search)
    {
        IQueryable<User> query = userRepository.Query();
        if(!string.IsNullOrEmpty(role))
            query = query.Where(u=>u.Role==role);
        if(!string.IsNullOrEmpty(search))
            query = query.Where(u=>u.FullName.Contains(search) || u.Email.Contains(search));
        return mapper.ProjectTo<UserDto>(query).ToListAsync();
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
       User? newUser = await userRepository.CreateAsync(user);
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