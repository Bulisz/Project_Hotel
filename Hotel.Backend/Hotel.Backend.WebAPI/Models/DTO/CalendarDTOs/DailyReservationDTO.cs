namespace Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;

public record DailyReservationDTO
{
    public int RoomNumber { get; set; }
    public string ReservationStatus { get; set; } = string.Empty;
    public string BookingFrom { get; set; } = string.Empty;
    public string BookingTo { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
