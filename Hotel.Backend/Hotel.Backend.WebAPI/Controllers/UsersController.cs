using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helper;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
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
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
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
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
    }

    //[Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<UserDetails>> GetCurrentUser()
    {
        try
        {
            string currentUserId = User.GetCurrentUserId();
            return (await _userService.GetUserByIdAsync(currentUserId))!;
        }
        catch (Exception)
        {
            return NoContent();
        }
    }

    [HttpPost(nameof(Register))]
    public async Task<ActionResult<UserDetails>> Register(CreateUserForm userDTOpost)
    {
        try
        {
            UserDetails userDTOget = await _userService.RegisterAsync(userDTOpost);
            return CreatedAtAction(nameof(GetUserByName), new { userName = userDTOget.Username }, userDTOget);
        }
        catch (HotelException ex)
        {
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
    }

    [HttpPost(nameof(VerifyEmail))]
    public async Task<ActionResult<bool>> VerifyEmail(EmailVerificationDTO request)
    {
        bool isSucceed = await _userService.VerifyEmailAsync(request);
        if (isSucceed)
        {
            return Ok(isSucceed);
        }
        return BadRequest(isSucceed);
    }

    [HttpPost(nameof(Login))]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            UserDetailsDTO userDTOlogin = await _userService.LoginAsync(request);
            LoginResponse response = _jwtService.CreateToken(userDTOlogin.User!, userDTOlogin.Roles);
            response.UserName = userDTOlogin.User.UserName;
            response.Id = userDTOlogin.User.Id;
            response.Role = userDTOlogin.Roles[0].ToString();
            response.Email = userDTOlogin.User.Email;
            response.FirstName = userDTOlogin.User.FirstName;
            response.LastName = userDTOlogin.User.LastName;
            return Ok(response);
        }
        catch (HotelException ex)
        {
            var error = (new {type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status,error);
        }
    }

    //[Authorize]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }

    //[Authorize]
    [HttpPut(nameof(UpdateUser))]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUser(UserUpdateDTO updateUser)
    {
        UserDetailsDTO userDetails = await _userService.UpdateUserAsync(updateUser);
        return userDetails;
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet(nameof(GetUsers))]
    public async Task<ActionResult<List<UserListItem>>> GetUsers()
    {
        List<UserListItem> users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut(nameof(UpdateUserAsAdmin))]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUserAsAdmin(UserDetailsForAdmin updateUser)
    {
        UserDetailsDTO newUser = await _userService.UpdateUserAsAdminAsync(updateUser);
        return Ok(newUser);
    }
}
