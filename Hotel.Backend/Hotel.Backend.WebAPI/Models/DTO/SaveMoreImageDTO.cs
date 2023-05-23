namespace Hotel.Backend.WebAPI.Models.DTO;

public record SaveMoreImageDTO
{
    public string RoomId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<IFormFile?> Images { get; set; } = new();
}
