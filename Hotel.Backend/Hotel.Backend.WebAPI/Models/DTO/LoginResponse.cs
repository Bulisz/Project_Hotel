namespace Hotel.Backend.WebAPI.Models.DTO;

public record LoginResponse
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}