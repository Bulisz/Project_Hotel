namespace Hotel.Backend.WebAPI.Models.DTO;

public record ReservationDetailsDTO
{
    public int Id { get; set; }
    public DateTime BookingFrom { get; set; }
    public DateTime BookingTo { get; set; }
    public int RoomId { get; set; }
    public string UserId { get; set; }

}