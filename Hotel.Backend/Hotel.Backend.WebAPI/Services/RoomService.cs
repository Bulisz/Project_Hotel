﻿using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

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
}