using Core.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedLibrary.Dtos;

namespace Core.Services;

public interface IUserService
{
    Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<AppUserDto>> GetUserByNameAsync(string userName);
    Task<Response<NoContent>> CreateUserRoels(string email);
}