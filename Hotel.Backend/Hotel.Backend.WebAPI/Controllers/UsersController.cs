using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helper;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IJwtService jwtService, IUserService userService, ILogger<UsersController> logger)
    {
        _jwtService = jwtService;
        _userService = userService;
        _logger = logger;
    }


    [Authorize(Roles = "Admin")]
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
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize]
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
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<UserDetails>> GetCurrentUser()
    {
        try
        {
            string currentUserId = User.GetCurrentUserId();
            return (await _userService.GetUserByIdAsync(currentUserId))!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
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
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }


    [HttpPost(nameof(VerifyEmail))]
    public async Task<ActionResult<bool>> VerifyEmail(EmailVerificationDTO request)
    {
        try
        {
            bool isSucceed = await _userService.VerifyEmailAsync(request);
            if (isSucceed)
            {
                return Ok(isSucceed);
            }
            return BadRequest(isSucceed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
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
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut(nameof(UpdateUser))]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUser(UserUpdateDTO updateUser)
    {
        try
        {
            UserDetailsDTO userDetails = await _userService.UpdateUserAsync(updateUser);
            return userDetails;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Operator")]
    [HttpGet(nameof(GetUsers))]
    public async Task<ActionResult<List<UserListItem>>> GetUsers()
    {
        try
        {
            List<UserListItem> users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut(nameof(UpdateUserAsAdmin))]
    public async Task<ActionResult<UserDetailsDTO>> UpdateUserAsAdmin(UserDetailsForAdmin updateUser)
    {
        try
        {
            UserDetailsDTO newUser = await _userService.UpdateUserAsAdminAsync(updateUser);
            return Ok(newUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
