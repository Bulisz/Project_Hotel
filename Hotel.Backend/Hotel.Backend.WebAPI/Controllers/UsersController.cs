using Google.Apis.Auth;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helper;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppSettings _applicationSettings;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IOptions<AppSettings> applicationSettings, IJwtService jwtService, IUserService userService, ILogger<UsersController> logger)
    {
        _applicationSettings = applicationSettings.Value;
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

    [Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<UserDetails>> GetCurrentUser()
    {
        try
        {
            string currentUserId = User.GetCurrentUserId();
            return Ok(await _userService.GetUserByIdAsync(currentUserId))!;
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
    public async Task<ActionResult<TokensDTO>> Login(LoginRequest request)
    {
        try
        {
            UserDetailsDTO userDTOlogin = await _userService.LoginAsync(request);
            TokensDTO loginResponse = await _jwtService.CreateTokensAsync(userDTOlogin.User);
            return Ok(loginResponse);
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

    [Authorize]
    [HttpPost(nameof(Logout))]
    public ActionResult Logout(LogoutRefreshRequest logoutRequest)
    {
        _jwtService.ClearRefreshToken(logoutRequest.RefreshToken);
        return Ok();
    }

    [HttpPost(nameof(Refresh))]
    public async Task<ActionResult<TokensDTO>> Refresh(LogoutRefreshRequest refreshRequest)
    {
        try
        {
            TokensDTO authenticationResponse = await _jwtService.RenewTokensAsync(refreshRequest.RefreshToken);
            return Ok(authenticationResponse);
        }
        catch (JwtException)
        {
            return Forbid();
        }
    }

    [HttpPost(nameof(ForgotPassword))]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO request)
    {
        try
        {
            await _userService.ForgotPasswordAsync(request);
            return Ok();
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

    [HttpPost(nameof(ResetPassword))]
    public async Task<ActionResult<bool>> ResetPassword(ResetPasswordDTO request)
    {
        try
        {
            bool isSucceed = await _userService.ResetPasswordAsync(request);
            return Ok(isSucceed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost(nameof(LoginWithGoogle))]
    public async Task<ActionResult<TokensDTO>> LoginWithGoogle(GoogleLoginDTO login)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { _applicationSettings.client_id }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(login.Credential, settings);

        ApplicationUser? user = await _userService.FindByEmailAsync(payload.Email);

        if (user != null)
        {
            TokensDTO tokens = await _jwtService.CreateTokensAsync(user);

            return Ok(tokens);
        }
        else
        {
            CreateUserForm userToCreate = new()
            {
                UserName = string.Concat(payload.Email.Split('@')[0].Split(".")),
                Email = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
            };

            UserDetailsDTO createdUser = await _userService.RegisterGoogleUserAsync(userToCreate);
            TokensDTO tokens = await _jwtService.CreateTokensAsync(createdUser.User);

            return Ok(tokens);
        }

    }

}
