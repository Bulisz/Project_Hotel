using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly HotelDbContext _context;

    public EquipmentRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Equipment> CreateEquipmentAsync(Equipment equipment)
    {
        _context.Equipments.Add(equipment);
        await _context.SaveChangesAsync();
        return equipment;
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        Equipment equipment = await _context.Equipments.FindAsync(id);
        _context.Equipments.Remove(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Equipment>> GetStandardEquipmentAsync()
    {
        return await _context.Equipments
             .Where(equipment => equipment.IsStandard == true)
             .ToListAsync();
    }

    public async Task<IEnumerable<Equipment>> GetNonStandardEquipmentAsync()
    {
        return await _context.Equipments
            .Where(equipment => equipment.IsStandard == false)
            .ToListAsync();
    }

    public async Task AddEquipmentToRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        Room room = await _context.Rooms.FindAsync(equipmentAndRoomDTO.RoomId);
        Equipment equipment = await _context.Equipments.FindAsync(equipmentAndRoomDTO.EquipmentId);

        room.Equipments.Add(equipment);
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveEquipmentFromRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        Room room = await _context.Rooms.FindAsync(equipmentAndRoomDTO.RoomId);
        Equipment equipment = await _context.Equipments.FindAsync(equipmentAndRoomDTO.EquipmentId);

        room.Equipments.Remove(equipment);
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }
}