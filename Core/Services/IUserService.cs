using Core.Dtos;
using SharedLibrary.Dtos;

namespace Core.Services;

public interface IUserService
{
    Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<AppUserDto>> GetUserByNameAsync(string userName);
}