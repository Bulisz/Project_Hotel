using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);

        Task<List<Room>> GetBigEnoughRoomsAsync();
        Task<IEnumerable<Equipment>> GetNonStandardEquipmentAsync();
    }
}