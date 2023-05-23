namespace Hotel.Backend.WebAPI.Models.DTO;

public record SaveMoreImageDTO
{
    public int RoomId { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<IFormFile?> Images { get; set; } = new();
}
