namespace Hotel.Backend.WebAPI.Models.DTO;

public record DeleteImageDTO
{
    public string imageUrl {  get; set; } = string.Empty;
}
