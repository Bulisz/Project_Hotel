using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record PostCreateDTO
{
    [Required]
    [MinLength(2)]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [EnumDataType(typeof(Role))]
    [Required]
    public string Role { get; set; } = string.Empty;
} 
