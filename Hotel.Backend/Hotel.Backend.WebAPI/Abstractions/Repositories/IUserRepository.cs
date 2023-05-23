using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories;

public interface IUserRepository
{
    Task<UserDetailsDTO?> GetUserByIdAsync(string id);
    Task<UserDetailsDTO?> GetUserByNameAsync(string name);
    Task<UserDetailsDTO> InsertUserAsync(ApplicationUser user, string password);
    Task<UserDetailsDTO> LoginAsync(LoginRequest request);
}
