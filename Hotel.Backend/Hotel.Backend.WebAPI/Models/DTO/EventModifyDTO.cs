namespace Hotel.Backend.WebAPI.Models.DTO;

public record EventModifyDTO
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public string OldImageUrl { get; set; } = string.Empty;
}
