using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations.Include(reservation => reservation.ApplicationUser)
                                          .Include(reservation => reservation.Room)
                                          .ToListAsync();
    }
}
