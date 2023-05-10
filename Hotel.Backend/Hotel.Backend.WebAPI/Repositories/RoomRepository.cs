using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _context.Rooms
            .Include(room => room.Equipments)
            .Include(room => room.Images)
            .Include(room => room.Reservations)
            .ToListAsync();
    }

    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        return await _context.Rooms
            .Include(room => room.Equipments)
            .Include(room => room.Images)
            .Include(room => room.Reservations)
            .FirstOrDefaultAsync(room => room.Id == id);
    }
}
