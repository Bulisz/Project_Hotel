using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Repositories;

public class ReservationRepository : IReservationRepository
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
