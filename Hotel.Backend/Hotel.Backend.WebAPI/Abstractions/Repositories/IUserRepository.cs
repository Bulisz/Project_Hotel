using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories;

public interface IUserRepository
{
    Task DeleteUserAsync(string userId);
    Task<UserDetailsDTO?> GetUserByIdAsync(string id);
    Task<UserDetailsDTO?> GetUserByNameAsync(string name);
    Task<UserDetailsDTO> InsertUserAsync(ApplicationUser user, string password);
    Task<UserDetailsDTO> LoginAsync(LoginRequest request);
    Task<UserDetailsDTO> UpdateUserAsync(UserUpdateDTO updateUser);
    Task<List<UserListItem>> GetAllUsersAsync();
    Task<UserDetailsDTO> UpdateUserAsAdminAsync(UserDetails request);
}
