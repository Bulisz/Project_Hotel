﻿using Hotel.Backend.WebAPI.Abstractions;
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
            throw new HotelException(HttpStatusCode.BadRequest, string.Join('\n', createResult.Errors.ToList()));

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, Role.Guest.ToString());
        if (!identityResult.Succeeded)
            throw new HotelException(HttpStatusCode.BadRequest, "Failed to add user to role");

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
        ApplicationUser user = await _userManager.FindByNameAsync(request.UserName)
            ?? throw new HotelException(HttpStatusCode.BadRequest, "Invalid name or password");
        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new HotelException(HttpStatusCode.BadRequest, "Invalid name or password");

        UserLoginDTO userDTOlogin = new()
        {
            User = user,
            Roles = await _userManager.GetRolesAsync(user)
        };

        return userDTOlogin;
    }
}