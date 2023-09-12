namespace Hotel.Backend.WebAPI.Models.DTO;

public record LogoutRefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
