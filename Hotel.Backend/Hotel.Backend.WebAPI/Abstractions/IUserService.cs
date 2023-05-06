using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions;

public interface IUserService
{
    Task<UserDetails?> GetUserByIdAsync(string id);
    Task<UserDetails?> GetUserByNameAsync(string name);
    Task<UserLoginDTO> LoginAsync(LoginRequest userLoginRequest);
    Task<UserDetails> RegisterAsync(CreateUserForm userDTOpost);
}
