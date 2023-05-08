using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helper;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;

    public UsersController(IJwtService jwtService, IUserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }


    //[Authorize(Roles = "Admin")]
    [HttpGet("GetUserByName/{username}")]
    public async Task<ActionResult<UserDetails>> GetUserByName(string username)
    {
        try
        {
            UserDetails? user = await _userService.GetUserByNameAsync(username);
            if (user is null)
                return NoContent();
            else
                return Ok(user);
        }
        catch (HotelException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult<UserDetails>> GetUserById(string id)
    {
        try
        {
            UserDetails? user = await _userService.GetUserByIdAsync(id);
            if (user is null)
                return NoContent();
            else
                return Ok(user);
        }
        catch (HotelException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpGet("GetCurrentUser")]
    //[Authorize]
    public async Task<ActionResult<UserDetails>> GetCurrentUser()
    {
        string currentUserId = User.GetCurrentUserId();
        return (await _userService.GetUserByIdAsync(currentUserId))!;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDetails>> Register(CreateUserForm userDTOpost)
    {
        try
        {
            UserDetails userDTOget = await _userService.RegisterAsync(userDTOpost);
            return CreatedAtAction(nameof(GetUserByName), new { userName = userDTOget.UserName }, userDTOget);
        }
        catch (HotelException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            UserLoginDTO userDTOlogin = await _userService.LoginAsync(request);
            LoginResponse token = _jwtService.CreateToken(userDTOlogin.User!, userDTOlogin.Roles);
            return Ok(token);
        }
        catch (HotelException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }
}
