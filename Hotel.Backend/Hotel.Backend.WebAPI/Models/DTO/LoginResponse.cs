namespace Hotel.Backend.WebAPI.Models.DTO;

public record LoginResponse
{
    public string? Token { get; set; }

    public DateTime Expiration { get; set; }
}