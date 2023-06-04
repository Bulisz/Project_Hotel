using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record CreateRoomDTO
{
    [Required]
    [RegularExpression("^^[A-ZÖÜÓŐÚÉÁŰÍ]{1,}[a-z0-9öüóőúéáűí]{1,49}$$", ErrorMessage = "2 és 50 betűből és számból állhat")]
    public string Name { get; set; } = string.Empty;

    [Range(1000,100000, ErrorMessage = "1_000 és 100_000 közötti érték")]
    public int Price { get; set; }

    [Range(1, 10, ErrorMessage = "1 és 10 közötti érték")]
    public int NumberOfBeds { get; set; }

    [MinLength(10, ErrorMessage = "10 és 250 karakter között")]
    [MaxLength(250, ErrorMessage = "10 és 250 karakter között")]
    public string Description { get; set; } = string.Empty;

    [MinLength(5, ErrorMessage = "5-től 50 karakterig")]
    [MaxLength(50, ErrorMessage = "5-től 50 karakterig")]
    public string Size { get; set; } = string.Empty;

    [MinLength(100, ErrorMessage = "100 és 4000 karakter között")]
    [MaxLength(4000, ErrorMessage = "100 és 4000 karakter között")]
    public string LongDescription { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessage = "1 és 10 közötti érték")]
    public int MaxNumberOfDogs { get; set; }
}