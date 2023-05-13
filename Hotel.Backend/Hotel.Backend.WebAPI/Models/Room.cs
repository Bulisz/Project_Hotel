using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Backend.WebAPI.Models;

[Table("Rooms")]
public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int NumberOfBeds { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public int MaxNumberOfDogs { get; set; }
    public bool Available { get; set; }
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
