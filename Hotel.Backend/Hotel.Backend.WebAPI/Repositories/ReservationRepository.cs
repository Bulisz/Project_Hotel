using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly HotelDbContext _context;

    public ReservationRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Reservation> CreateReservationAsync(Reservation newReservation)
    {
        _context.Reservations.Add(newReservation);
        await _context.SaveChangesAsync();

        return newReservation;
    }
}
