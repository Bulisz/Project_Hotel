using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservationAsync(Reservation newReservation);
        Task<List<Reservation>> GetAllReservationsAsync();
    }
}