namespace Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;

public record ThisMonthCalendarDTO
{
    public DateTime Day { get; set; }
    public int DateNumber { get; set; }
    public int WeekDayNumber { get; set; }
    public List<DailyReservationDTO> RoomStatus { get; set; } = new List<DailyReservationDTO>();
}
