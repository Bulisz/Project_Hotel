using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;

namespace Hotel.Backend.WebAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public UserRepository(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<UserDetailsDTO> InsertUserAsync(ApplicationUser user, string password)
    {
        IdentityResult createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            List<HotelFieldError> errors = createResult.Errors.Select(err => 
                new HotelFieldError(err.Description.Substring(0, err.Description.IndexOf(" ")) == "Username" ? "UserName" : 
                err.Description.Substring(0, err.Description.IndexOf(" ")) == "Passwords" ? "Password" : err.Description.Substring(0, err.Description.IndexOf(" ")), err.Description)).ToList();
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

        var emailVerificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedVerificationToken = Encoding.UTF8.GetBytes(emailVerificationToken);
        var validEmailToken = WebEncoders.Base64UrlEncode(encodedVerificationToken);
        string url = $"http://bulisz-001-site1.dtempurl.com//#/confirmEmail/?email={user.Email}&token={validEmailToken}";


        EmailDTO email = _emailService.CreatingVerificationEmail(user.Email, user.LastName, user.FirstName, url);

        await _emailService.SendEmailAsync(email);
            

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, Role.Guest.ToString());
        if (!identityResult.Succeeded)
        {
            List<HotelFieldError> errors = identityResult.Errors.Select(err => new HotelFieldError(err.Description.Substring(0, err.Description.IndexOf(" ")), err.Description)).ToList();
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

        UserDetailsDTO userDetails = new UserDetailsDTO();
        userDetails.User = user;
        userDetails.Roles = await _userManager.GetRolesAsync(user);

        return userDetails;
    }

    public async Task<bool> VerifyEmailAsync(EmailVerificationDTO emailVerification)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(emailVerification.Email);
        if(user != null)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(emailVerification.Token);
            string token = Encoding.UTF8.GetString(decodedToken);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<UserDetailsDTO?> GetUserByNameAsync(string name)
    {
        UserDetailsDTO userDetailsDTO = new UserDetailsDTO();
        ApplicationUser? user = await _userManager.FindByNameAsync(name);
        userDetailsDTO.User = user;
        userDetailsDTO.Roles = await _userManager.GetRolesAsync(user);

        return userDetailsDTO;
    }

    public async Task<UserDetailsDTO> GetUserByIdAsync(string id)
    {
        UserDetailsDTO userDetailsDTO = new UserDetailsDTO();
        ApplicationUser? user = await _userManager.FindByIdAsync(id);
        userDetailsDTO.User = user;
        userDetailsDTO.Roles = await _userManager.GetRolesAsync(user);

        return userDetailsDTO;
    }

    public async Task<UserDetailsDTO> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("UserName", "A felhasználónév vagy a jelszó érvénytelen"), new HotelFieldError("Password", "A felhasználónév vagy a jelszó érvénytelen") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        };

        var isEmailValid = await _userManager.IsEmailConfirmedAsync(user);
        if(!isEmailValid)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Password", "Az email-cím nincs megerősítve") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occured.");
        }

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("UserName", "A felhasználónév vagy a jelszó érvénytelen"), new HotelFieldError("Password", "A felhasználónév vagy a jelszó érvénytelen") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        }

        UserDetailsDTO userDTOlogin = new()
        {
            User = user,
            Roles = await _userManager.GetRolesAsync(user)
        };

        return userDTOlogin;
    }

    public async Task DeleteUserAsync(string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
        await _userManager.DeleteAsync(user);
    }

    public async Task<UserDetailsDTO> UpdateUserAsync(UserUpdateDTO updateUser)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(updateUser.Id);
        user.UserName = updateUser.Username;
        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;
        user.Email = updateUser.Email;
        if(user is not null)
        {
            await _userManager.UpdateAsync(user);
        }

        UserDetailsDTO userDetails = new UserDetailsDTO();
        userDetails.User = user;
        userDetails.Roles = await _userManager.GetRolesAsync(user);

        return userDetails;
    }

    public async Task<List<UserListItem>> GetAllUsersAsync()
    {
        List<ApplicationUser> users = await _userManager.Users.OrderBy(user => user.LastName).ToListAsync();

        List<UserListItem> listedUsers = users.Select(user => new UserListItem
        {
            Id = user.Id,
            Username = user.UserName!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed.ToString(),
        }).ToList();

        foreach(var listedUser  in listedUsers)
        {
            var user = await _userManager.FindByIdAsync(listedUser.Id);
            var roles = await _userManager.GetRolesAsync(user);
            listedUser.Role = roles[0];
        }

        return listedUsers;
    }

    public async Task<UserDetailsDTO> UpdateUserAsAdminAsync(UserDetailsForAdmin request)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(request.Id);
        
            await ChangeRoleAsync(request.Role, user);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.EmailConfirmed = Convert.ToBoolean(request.EmailConfirmed);
            user.UserName = request.Username;

            await _userManager.UpdateAsync(user);
        

        UserDetailsDTO userDetails = new UserDetailsDTO();
        userDetails.User = user;
        userDetails.Roles = await _userManager.GetRolesAsync(user);

        return userDetails;
    }

    private async Task ChangeRoleAsync(string newRole, ApplicationUser user)
    {
        var usersRoles = await _userManager.GetRolesAsync(user);
        string oldRole = usersRoles[0];

        if (oldRole != newRole)
        {
            await _userManager.RemoveFromRoleAsync(user, oldRole);
            await _userManager.AddToRoleAsync(user, newRole);
        }
    }
}
