using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoomsAsync();
    }
}