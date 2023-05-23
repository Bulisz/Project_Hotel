using Hotel.Backend.WebAPI.Abstractions.Repositories;
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

    public async Task DeleteReservationAsync(int reservationId)
    {
        Reservation reservation = await _context.Reservations.Where(reservation => reservation.Id == reservationId).FirstOrDefaultAsync()
            ?? throw new ArgumentException("Nincs ilyen foglalás");
        _context.Reservations.Remove(reservation);
        _context.SaveChanges();
    }

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations.Include(reservation => reservation.ApplicationUser)
                                          .Include(reservation => reservation.Room)
                                          .OrderBy(reservation => reservation.BookingFrom)
                                          .ToListAsync();
    }

    public async Task<List<Reservation>> GetMyReservationsAsync(string userId)
    {
        return await _context.Reservations.Include(reservation => reservation.ApplicationUser)
                                          .Include(reservation => reservation.Room)
                                          .Where(reservation => reservation.ApplicationUser.Id == userId)
                                          .OrderBy(reservation => reservation.BookingFrom)
                                          .ToListAsync();
    }
}
