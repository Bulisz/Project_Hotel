using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using System.Net;

namespace Hotel.Backend.WebAPI.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    private readonly Cloudinary _cloudinary;

    public RoomService(IRoomRepository roomRepository, IMapper mapper, Cloudinary cloudinary)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
        _cloudinary = cloudinary;
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
        if (room == null)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Id", "Room id is invalid") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        };

        RoomDetailsDTO roomDetails = _mapper.Map<RoomDetailsDTO>(room);

        return roomDetails;
    }

    public async Task<IEnumerable<RoomListDTO>> GetAvailableRoomsAsync(RoomSelectorDTO query)
    {
        if (query.BookingFrom < query.BookingTo)
        {
            int guestNumber = query.NumberOfBeds;
            int dogNumber = query.MaxNumberOfDogs;
            List<int> choosedEquipmentsId = query.ChoosedEquipments;
            DateTime bookingFrom = query.BookingFrom;
            DateTime bookingTo = query.BookingTo;

            List<Room> allRooms = await _roomRepository.GetBigEnoughRoomsAsync(
                guestNumber, dogNumber, choosedEquipmentsId, bookingFrom, bookingTo);
            List<Room> onlyRequestedRooms = new();
            int counter = 0;
            if (choosedEquipmentsId.Count != 0 && choosedEquipmentsId != null)
            {
                foreach (int item in choosedEquipmentsId)
                {
                    foreach (Room room in allRooms)
                    {
                        foreach (var equipment in room.Equipments)
                        {
                            if (item == equipment.Id)
                            {
                                counter++;
                            }
                        }
                        if (counter == choosedEquipmentsId.Count)
                        {
                            onlyRequestedRooms.Add(room);
                            counter = 0;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in allRooms)
                {
                    onlyRequestedRooms.Add(item);
                }
            }
            List<RoomListDTO> bigEnoughrooms = _mapper.Map<List<RoomListDTO>>(onlyRequestedRooms);

            return bigEnoughrooms;
        }

        List<HotelFieldError> errors = new() { new HotelFieldError("BookingTo", "A távozásnak később kell lennie, mint az érkezésnek"), new HotelFieldError("BookingFrom", "A távozásnak később kell lennie, mint az érkezésnek") };
        throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");

    }

    public async Task<IEnumerable<NonStandardEquipmentDTO>> GetNonStandardEquipmentsAsync()
    {
        IEnumerable<Equipment> nonStandardEquipment = await _roomRepository.GetNonStandardEquipmentAsync();
        List<NonStandardEquipmentDTO> nonStandardEquipmentDTO = _mapper.Map<List<NonStandardEquipmentDTO>>(nonStandardEquipment);
        return nonStandardEquipmentDTO;
    }

    public async Task SaveOneImageAsync(SaveOneImageDTO saveOneImage)
    {
        Room room = await _roomRepository.GetRoomByIdAsync(saveOneImage.RoomId);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(saveOneImage.Image?.Name, saveOneImage.Image?.OpenReadStream()),
            PublicId = Guid.NewGuid().ToString(),
            Folder = $"Hotel/Room{saveOneImage.RoomId}",
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        var imageurl = uploadResult.SecureUrl.ToString();

        Image newImage = new Image
        {
            Description = saveOneImage.Description,
            ImageUrl = imageurl,
            Room = room
        };

        await _roomRepository.SaveOneImageAsync(newImage);
    }

    public async Task SaveMoreImageAsync(SaveMoreImageDTO saveMoreImage)
    {
        Room room = await _roomRepository.GetRoomByIdAsync(saveMoreImage.RoomId);
        List<Image> images = new();

        foreach (var actualImage in saveMoreImage.Images)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(actualImage?.Name, actualImage?.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
                Folder = $"Hotel/Room{saveMoreImage.RoomId}",
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            var imageurl = uploadResult.SecureUrl.ToString();

            Image image = new Image
            {
                Description = saveMoreImage.Description,
                ImageUrl = imageurl,
                Room = room
            };

            images.Add(image);
        }

        await _roomRepository.SaveMoreImageAsync(images);
    }
}
