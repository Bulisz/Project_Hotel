using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Backend.WebAPI.Models;

[Table("Equipments")]
public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsStandard { get; set; }

    public ICollection<Room> Rooms { get; set; }
}
