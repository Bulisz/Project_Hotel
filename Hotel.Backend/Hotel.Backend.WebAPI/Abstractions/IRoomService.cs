using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomListDTO>> GetAvailableRoomsAsync();
        Task<IEnumerable<RoomListDTO>> GetListOfRoomsAsync();
        
        Task<IEnumerable<NonStandardEquipmentDTO>> GetNonStandardEquipmentsAsync();
        Task<RoomDetailsDTO> GetRoomByIdAsync(int id);
    }
}