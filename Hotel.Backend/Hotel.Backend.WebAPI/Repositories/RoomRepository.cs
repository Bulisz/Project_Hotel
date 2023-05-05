using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Microsoft.AspNetCore.Http.Connections;

namespace Hotel.Backend.WebAPI.Repositories;

public class RoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context) {
        _context = context;
    }

}
