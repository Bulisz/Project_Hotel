using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IStatisticsService
{
    Task<IEnumerable<RoomReservationPerMonthDTO>> GetRoomMonthStatAsync(int year, int month);
    Task<IEnumerable<StatisticsPerYearDTO>> GetYearStatAsync(YearStatQueryDTO query);
}
