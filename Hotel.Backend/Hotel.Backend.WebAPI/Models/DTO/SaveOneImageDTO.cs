namespace Hotel.Backend.WebAPI.Models.DTO;

public record SaveOneImageDTO
{
    public string RoomId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}
