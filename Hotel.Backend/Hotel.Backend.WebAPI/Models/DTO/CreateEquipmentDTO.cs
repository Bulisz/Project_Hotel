namespace Hotel.Backend.WebAPI.Models.DTO;

public record CreateEquipmentDTO
{
    public string Name { get; set; } = string.Empty;
    public bool IsStandard { get; set; }
}