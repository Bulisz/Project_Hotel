using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UserService(IUserRepository userRepository, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UserDetails> RegisterAsync(CreateUserForm userDTOpost)
    {
        ApplicationUser userToRegister = _mapper.Map<ApplicationUser>(userDTOpost);
        userToRegister.CreatedAt = _dateTimeProvider.Now;

        UserDetailsDTO createdUser = await _userRepository.InsertUserAsync(userToRegister, userDTOpost.Password);

        return _mapper.Map<UserDetails>(createdUser);
    }

    public async Task<UserDetails?> GetUserByNameAsync(string name)
    {
        UserDetailsDTO? userDetailsDTO = await _userRepository.GetUserByNameAsync(name);

        return _mapper.Map<UserDetails>(userDetailsDTO);
    }

    public async Task<UserDetails?> GetUserByIdAsync(string id)
    {
        UserDetailsDTO? userDetailsDTO = await _userRepository.GetUserByIdAsync(id);

        return _mapper.Map<UserDetails>(userDetailsDTO);
    }

    public async Task<UserDetailsDTO> LoginAsync(LoginRequest userLoginRequest)
    {
        return await _userRepository.LoginAsync(userLoginRequest);
    }

    public async Task DeleteUserAsync(string userId)
    {
        await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<UserDetailsDTO> UpdateUserAsync(UserUpdateDTO updateUser)
    {
        UserDetailsDTO userDetails = await _userRepository.UpdateUserAsync(updateUser);
        return userDetails;
    }

    public async Task<List<UserListItem>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<UserDetailsDTO> UpdateUserAsAdminAsync(UserDetailsForAdmin updateUser)
    {
        UserDetailsDTO newUser = await _userRepository.UpdateUserAsAdminAsync(updateUser);
        return newUser;
    }

    public async Task<bool> VerifyEmailAsync(EmailVerificationDTO request)
    {
        bool isSucceeded = await _userRepository.VerifyEmailAsync(request);
        return isSucceeded;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDTO request)
    {
        await _userRepository.ForgotPasswordAsync(request);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDTO request)
    {
        bool isSucceed = await _userRepository.ResetPasswordAsync(request);
        return isSucceed;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userRepository.FindByEmailAsync(email);
        
    }

    public async Task<UserDetailsDTO> RegisterGoogleUserAsync(CreateUserForm userToCreate)
    {
        ApplicationUser userToRegister = _mapper.Map<ApplicationUser>(userToCreate);
        userToRegister.CreatedAt = _dateTimeProvider.Now;

        UserDetailsDTO createdUser = await _userRepository.InsertGoogleUserAsync(userToRegister);

        return _mapper.Map<UserDetailsDTO>(createdUser);
    }
}
