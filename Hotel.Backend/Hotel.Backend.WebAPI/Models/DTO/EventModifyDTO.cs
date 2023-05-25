using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record EventModifyDTO
{
    public string Id { get; set; } = string.Empty;

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

    public IFormFile? Image { get; set; }

    public string OldImageUrl { get; set; } = string.Empty;
}
