using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

    public async Task<List<Room>> GetBigEnoughRoomsAsync(int guestNumber, int dogNumber, List<int> choosedEquipments)
    {
        return await _context.Rooms
            .Include(room => room.Equipments)
            .Include(room => room.Images)
            .Include(room => room.Reservations)
            .Where(room => room.NumberOfBeds >= guestNumber)
            .Where(room => room.MaxNumberOfDogs >= dogNumber)
            .Where(room => room.Equipments.Select(eq => eq.Id).ToList().All(id => choosedEquipments.ToList().Any(c => c == id)))
            .ToListAsync();



        



    }

    public async Task<IEnumerable<Equipment>> GetNonStandardEquipmentAsync()
    { 
        return await _context.Equipments
            .Where(equipment => equipment.IsStandard == false)
            .ToListAsync();
    }
}
