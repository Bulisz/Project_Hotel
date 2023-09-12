namespace Hotel.Backend.WebAPI.Models.DTO.OptionDTOs;

public record JwtTokensOptions
{
    public JwtTokenOptions AccessTokenOptions { get; set; } = new JwtTokenOptions();
    public JwtTokenOptions RefreshTokenOptions { get; set; } = new JwtTokenOptions();
}
