namespace Hotel.Backend.WebAPI.Models.DTO;

public record TokensDTO
{
    public JwtToken AccessToken { get; set; } = null!;
    public JwtToken RefreshToken { get; set; } = null!;
}
