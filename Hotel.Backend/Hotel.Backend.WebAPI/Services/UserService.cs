using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDetails> RegisterAsync(CreateUserForm userDTOpost)
    {
        ApplicationUser userToRegister = _mapper.Map<ApplicationUser>(userDTOpost);

        ApplicationUser createdUser = await _userRepository.InsertUserAsync(userToRegister, userDTOpost.Password);

        return _mapper.Map<UserDetails>(createdUser);
    }

    public async Task<UserDetails?> GetUserByNameAsync(string name)
    {
        ApplicationUser? user = await _userRepository.GetUserByNameAsync(name);

        return _mapper.Map<UserDetails>(user);
    }

    public async Task<UserDetails?> GetUserByIdAsync(string id)
    {
        ApplicationUser? user = await _userRepository.GetUserByIdAsync(id);

        return _mapper.Map<UserDetails>(user);
    }

    public async Task<UserLoginDTO> LoginAsync(LoginRequest userLoginRequest)
    {
        return await _userRepository.LoginAsync(userLoginRequest);
    }
}
