using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hotel.Backend.WebAPI.Services;

public class JwtService : IJwtService
{
    private const int EXPIRATION_MINUTES = 30;

    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LoginResponse CreateToken(IdentityUser user, IList<string> roles)
    {
        var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);

        var token = CreateJwtToken(
            CreateClaims(user, roles),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new LoginResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration) =>
        new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private Claim[] CreateClaims(IdentityUser user, IList<string> roles) =>
        new[] {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        }
        .Union(roles.Select(role => new Claim(ClaimTypes.Role, role)))
        .ToArray();

    private SigningCredentials CreateSigningCredentials() =>
        new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256);


}
