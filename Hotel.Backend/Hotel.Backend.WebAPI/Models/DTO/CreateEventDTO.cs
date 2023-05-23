namespace Hotel.Backend.WebAPI.Models.DTO;

public record CreateEventDTO
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}