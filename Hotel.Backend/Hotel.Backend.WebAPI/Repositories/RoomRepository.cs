﻿using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
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
            .Where(room => room.Available == true)
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

    public async Task<List<Room>> GetBigEnoughRoomsAsync(int guestNumber, int dogNumber, 
        List<int> choosedEquipments, DateTime bookingFrom, DateTime bookingTo)
    {
        List<Room> result = await _context.Rooms
            .Include(room => room.Equipments)
            .Include(room => room.Images)
            .Include(room => room.Reservations)
            .Where(room => room.NumberOfBeds >= guestNumber)
            .Where(room => room.MaxNumberOfDogs >= dogNumber)
            .Where(room => room.Available == true)
            //.Where(room => choosedEquipments.All(x => room.Equipments.Select(e => e.Id).Contains(x)))
            //.Where(room => choosedEquipments.All(x => room.Equipments.Any(e => e.Id == x)))
            .Where(room => !room.Reservations.Any(ar => ar.BookingFrom < bookingTo && ar.BookingTo > bookingFrom))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Equipment>> GetNonStandardEquipmentAsync()
    { 
        return await _context.Equipments
            .Where(equipment => equipment.IsStandard == false)
            .ToListAsync();
    }

    public async Task SaveOneImageAsync(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task SaveMoreImageAsync(List<Image> images)
    {
        _context.Images.AddRange(images);
        await _context.SaveChangesAsync();
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        room.Available = true;
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task DeleteRoomAsync(int id)
    {
        Room room = await _context.Rooms.FindAsync(id);
        room.Available = false;
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task<Room> ModifyRoomAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
        return room;
    }
}
