namespace Hotel.Backend.WebAPI.Models.DTO;

public class LoginResponse
{
    public string? Token { get; set; }

    public DateTime Expiration { get; set; }
}