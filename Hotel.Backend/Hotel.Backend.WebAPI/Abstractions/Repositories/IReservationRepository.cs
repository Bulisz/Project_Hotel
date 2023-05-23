using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservationAsync(Reservation newReservation);
        Task DeleteReservationAsync(int reservationId);
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<List<Reservation>> GetMyReservationsAsync(string userId);
    }
}