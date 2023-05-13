using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Hotel.Backend.WebAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> InsertUserAsync(ApplicationUser user, string password)
    {
        IdentityResult createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            List<HotelFieldError> errors = createResult.Errors.Select(err => 
                new HotelFieldError(err.Description.Substring(0, err.Description.IndexOf(" ")) == "Username" ? "UserName" : 
                err.Description.Substring(0, err.Description.IndexOf(" ")) == "Passwords" ? "Password" : err.Description.Substring(0, err.Description.IndexOf(" ")), err.Description)).ToList();
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }
            

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, Role.Guest.ToString());
        if (!identityResult.Succeeded)
        {
            List<HotelFieldError> errors = identityResult.Errors.Select(err => new HotelFieldError(err.Description.Substring(0, err.Description.IndexOf(" ")), err.Description)).ToList();
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

        return user;
    }

    public async Task<ApplicationUser?> GetUserByNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<UserLoginDTO> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("UserName", "UserName is invalid") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        };
        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Password", "Password is invalid") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

        UserLoginDTO userDTOlogin = new()
        {
            User = user,
            Roles = await _userManager.GetRolesAsync(user)
        };

        return userDTOlogin;
    }
}
