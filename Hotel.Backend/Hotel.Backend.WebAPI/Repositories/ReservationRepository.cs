using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Repositories
{
    public class ReservationRepository
    {
        private readonly HotelDbContext _context;

        public ReservationRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation newReservation)
        {
            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            return newReservation;
        }
    }
}
