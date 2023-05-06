using Hotel.Backend.WebAPI.Database;

namespace Hotel.Backend.WebAPI.Repositories;

public class RoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context) {
        _context = context;
    }

}
