namespace Hotel.Backend.WebAPI.Models.DTO;

public record RoomReservationPerMonthDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Percentage { get; set; }
}
