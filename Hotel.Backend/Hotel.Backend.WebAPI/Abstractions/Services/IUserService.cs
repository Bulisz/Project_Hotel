using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IUserService
{
    Task DeleteUserAsync(string userId);
    Task<UserDetails?> GetUserByIdAsync(string id);
    Task<UserDetails?> GetUserByNameAsync(string name);
    Task<UserDetailsDTO> LoginAsync(LoginRequest userLoginRequest);
    Task<UserDetails> RegisterAsync(CreateUserForm userDTOpost);
    Task<UserDetailsDTO> UpdateUserAsync(UserUpdateDTO updateUser);
    Task<List<UserListItem>> GetAllUsersAsync();
    Task<UserDetailsDTO> UpdateUserAsAdminAsync(UserDetailsForAdmin updateUser);
    Task<bool> VerifyEmailAsync(EmailVerificationDTO request);
    Task ForgotPasswordAsync(ForgotPasswordDTO request);
    Task<bool> ResetPasswordAsync(ResetPasswordDTO request);
}
