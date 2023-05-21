namespace Hotel.Backend.WebAPI.Models.DTO;

public class SaveOneImageDTO
{
    public int RoomId { get; set; }
    public string Description { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}
