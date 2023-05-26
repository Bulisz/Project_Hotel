using System.ComponentModel.DataAnnotations;

namespace Hotel.Backend.WebAPI.Models.DTO;

public record CreateRoomDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public int NumberOfBeds { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public int MaxNumberOfDogs { get; set; }
}