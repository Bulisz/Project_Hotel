using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservationAsync(Reservation newReservation);
    }
}