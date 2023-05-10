using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using System.Net;

namespace Hotel.Backend.WebAPI.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomListDTO>> GetListOfRoomsAsync()
    {
        List<Room> allRooms = await _roomRepository.GetAllRoomsAsync();

        List<RoomListDTO> rooms = _mapper.Map<List<RoomListDTO>>(allRooms);

        return rooms;



    }

    public async Task<RoomDetailsDTO> GetRoomByIdAsync(int id)
    {
        Room room = await _roomRepository.GetRoomByIdAsync(id)
            ?? throw new HotelException(HttpStatusCode.BadRequest, "Invalid room id");

        RoomDetailsDTO roomDetails = _mapper.Map<RoomDetailsDTO>(room);

        return roomDetails;
    }

    public async Task<IEnumerable<RoomListDTO>> GetAvailableRoomsAsync(int guestNumber, int dogNumber)
    {
        List<Room> allRooms = await _roomRepository.GetBigEnoughRoomsAsync(guestNumber);

        

        List<RoomListDTO> rooms = _mapper.Map<List<RoomListDTO>>(allRooms);

        return rooms;



    }
}
