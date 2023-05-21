namespace Hotel.Backend.WebAPI.Models.DTO;

public record LoginResponse
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}