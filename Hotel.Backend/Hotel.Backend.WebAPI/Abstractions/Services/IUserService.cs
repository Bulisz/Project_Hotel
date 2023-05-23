using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IUserService
{
    Task<UserDetails?> GetUserByIdAsync(string id);
    Task<UserDetails?> GetUserByNameAsync(string name);
    Task<UserDetailsDTO> LoginAsync(LoginRequest userLoginRequest);
    Task<UserDetails> RegisterAsync(CreateUserForm userDTOpost);
}
