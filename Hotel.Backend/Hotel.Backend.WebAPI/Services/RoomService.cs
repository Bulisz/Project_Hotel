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
        Room? room = await _roomRepository.GetRoomByIdAsync(id);
        if(room == null)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Id", "Room id is invalid") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        };

        RoomDetailsDTO roomDetails = _mapper.Map<RoomDetailsDTO>(room);

        return roomDetails;
    }

    public async Task<IEnumerable<RoomListDTO>> GetAvailableRoomsAsync(RoomSelectorDTO query)
    {
        int guestNumber = int.Parse(query.GuestNumber);
        int dogNumber = int.Parse(query.DogNumber);
        string[] choosedEquipments = query.ChoosedEquipments.Split(",");
        List<int> choosedEquipmentsId = new List<int>();
        foreach (var item in choosedEquipments)
        {
            choosedEquipmentsId.Add(int.Parse(item));
        }
       
        
        List<Room> allRooms = await _roomRepository.GetBigEnoughRoomsAsync(guestNumber, dogNumber, choosedEquipmentsId);

        

        List<RoomListDTO> bigEnoughrooms = _mapper.Map<List<RoomListDTO>>(allRooms);

        return bigEnoughrooms;



    }

    public async Task<IEnumerable<NonStandardEquipmentDTO>> GetNonStandardEquipmentsAsync()
    {
        IEnumerable<Equipment> nonStandardEquipment = await _roomRepository.GetNonStandardEquipmentAsync();
        List<NonStandardEquipmentDTO> nonStandardEquipmentDTO = _mapper.Map<List<NonStandardEquipmentDTO>>(nonStandardEquipment);
        return nonStandardEquipmentDTO;
    }
}
