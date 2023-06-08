namespace Hotel.Backend.WebAPI.Models.DTO;

public record GoogleLoginDTO
{
    public string Credential { get; set; } = string.Empty;
}
