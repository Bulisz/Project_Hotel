using Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface ICalendarService
{
    Task<List<ThisMonthCalendarDTO>> GetAllDaysOfMonthAsync(int year, int month);
}