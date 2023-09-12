using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories;

public interface IReservationRepository
{
    Task<Reservation> CreateReservationAsync(Reservation newReservation);
    Task<Reservation> DeleteReservationAsync(int reservationId);
    Task<List<Reservation>> GetAllReservationsAsync();
    Task<List<Reservation>> GetMyReservationsAsync(string userId);
}