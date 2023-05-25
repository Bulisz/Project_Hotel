using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record CreateEventDTO
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(150)]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Schedule { get; set; } = string.Empty;
    [Required]
    public IFormFile? Image { get; set; }
}