using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);
        Task<List<Room>> GetBigEnoughRoomsAsync(int guestNumber, int dogNumebr, List<int> choosedEquipments,
            DateTime bookingFrom, DateTime bookingTo);
        Task<IEnumerable<Equipment>> GetNonStandardEquipmentAsync();
        Task SaveOneImageAsync(Image image);
        Task SaveMoreImageAsync(List<Image> images);
        Task<Room> CreateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<Room> ModifyRoomAsync(Room room);
    }
}