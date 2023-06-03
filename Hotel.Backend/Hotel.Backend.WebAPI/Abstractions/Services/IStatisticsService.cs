using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IStatisticsService
    {
        Task<IEnumerable<RoomReservationPerMonthDTO>> GetRoomMonthStatAsync(int year, int month);
    }
}
