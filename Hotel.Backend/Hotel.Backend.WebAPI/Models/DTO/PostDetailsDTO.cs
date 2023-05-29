namespace Hotel.Backend.WebAPI.Models.DTO;

public record PostDetailsDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool Confirmed { get; set; }
    public string Role { get; set; } = string.Empty;
}
