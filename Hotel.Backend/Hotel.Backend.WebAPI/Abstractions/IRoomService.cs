using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomListDTO>> GetAvailableRoomsAsync(int guestNumber, int dogNumber);
        Task<IEnumerable<RoomListDTO>> GetListOfRoomsAsync();
        Task<RoomDetailsDTO> GetRoomByIdAsync(int id);
    }
}