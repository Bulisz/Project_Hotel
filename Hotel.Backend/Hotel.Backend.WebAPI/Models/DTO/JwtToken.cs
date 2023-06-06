namespace Hotel.Backend.WebAPI.Models.DTO;

public record JwtToken
{
    public string? Value { get; set; }

    public DateTime Expiration { get; set; }
}
