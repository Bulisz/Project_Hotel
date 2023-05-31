namespace Hotel.Backend.WebAPI.Models.DTO;

public record EmailVerificationDTO
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
