using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenCreationService _jwtService;

    public UsersController(UserManager<ApplicationUser> userManager, ITokenCreationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<CreateUserForm>> PostUser(CreateUserForm userForm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ApplicationUser user = new()
        {
            UserName = userForm.UserName,
            Email = userForm.Email,
            FirstName = userForm.FirstName,
            LastName = userForm.LastName,
        };

        var result = await _userManager.CreateAsync(
            user,
            userForm.Password
        );

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, role: "Admin");

        if (!identityResult.Succeeded)
        {
            throw new Exception("Failed to add user to role");
        }

        return CreatedAtAction(nameof(GetUser), new { userName = user.UserName }, user);
    }

    // GET: api/Users/username
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDetails>> GetUser(string username)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return new UserDetails
        {
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName= user.FirstName,
            LastName= user.LastName,
        };
    }

    // POST: api/Users/BearerToken
    [HttpPost("BearerToken")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Bad credentials");
        }

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.CreateToken(user, roles);

        return Ok(token);
    }
}
