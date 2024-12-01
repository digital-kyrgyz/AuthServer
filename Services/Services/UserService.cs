using Core.Dtos;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;

namespace Services.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new AppUser { Email = createUserDto.Email, UserName = createUserDto.UserName };
        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<AppUserDto>.Fail(new ErrorDto(errors, true), 400);
        }

        return Response<AppUserDto>.Success(ObjectMapper.Mapper.Map<AppUserDto>(user), 200);
    }

    public async Task<Response<AppUserDto>> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return Response<AppUserDto>.Fail("UserName is not found", 404, true);
        }

        return Response<AppUserDto>.Success(ObjectMapper.Mapper.Map<AppUserDto>(user), 200);
    }

    public async Task<Response<NoContent>> CreateUserRoels(string email)
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Manager"));
        }

        var user = await _userManager.FindByEmailAsync(email);

        await _userManager.AddToRoleAsync(user!, "Admin");
        await _userManager.AddToRoleAsync(user!, "Manager");

        return Response<NoContent>.Success(StatusCodes.Status201Created);
    }
}