using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;
using Ecommerce.api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.api.Service;

public interface IUserService
{
    Task<List<UserDto>> List(string role,string search);
    Task<UserDto> GetUser(Guid id);
    Task<SessionDto>Authorize(LoginDto model);
    Task<UserDto>Create(UserDto model);
    Task<bool>Update(UserDto model);
    Task<bool>Delete(Guid id);
}
public class UserService(IGenericRepository<User> userRepository,IMapper mapper):IUserService
{
    private readonly IGenericRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    
    public Task<List<UserDto>> List(string role, string search)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<SessionDto> Authorize(LoginDto model)
    {
        try
        {
            IQueryable<User> query= _userRepository.Query(u=>u.Email==model.Email && u.Password==model.Password);
            User? user = await query.FirstOrDefaultAsync();
            if(user==null)
            {
                throw new Exception("Invalid email or password");
            } 
            return _mapper.Map<SessionDto>(user);
        }catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public Task<UserDto> Create(UserDto model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(UserDto model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}