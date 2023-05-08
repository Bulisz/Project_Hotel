using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByIdAsync(string id);
    Task<ApplicationUser?> GetUserByNameAsync(string name);
    Task<ApplicationUser> InsertUserAsync(ApplicationUser user, string password);
    Task<UserLoginDTO> LoginAsync(LoginRequest request);
}
