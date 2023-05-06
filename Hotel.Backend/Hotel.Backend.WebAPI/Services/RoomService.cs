using Hotel.Backend.WebAPI.Abstractions;

namespace Hotel.Backend.WebAPI.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
}
