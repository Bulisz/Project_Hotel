using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IJwtService
{
    void ClearRefreshToken(string refreshToken);
    Task<TokensDTO> CreateTokensAsync(ApplicationUser user);
    Task<TokensDTO> RenewTokensAsync(string refreshToken);

}
