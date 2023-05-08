using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomListDTO>> GetListOfRoomsAsync();
    }
}