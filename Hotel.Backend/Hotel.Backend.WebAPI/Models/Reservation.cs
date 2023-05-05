using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Backend.WebAPI.Models;

[Table("Reservations")]
public class Reservation
{
    public int Id { get; set; }
    public DateTime BookingFrom { get; set; }
    public DateTime BookingTo { get; set; }
    public Room Room { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
