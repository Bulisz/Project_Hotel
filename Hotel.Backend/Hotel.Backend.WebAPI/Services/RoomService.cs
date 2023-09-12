using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using System.Net;
using System.Text;

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
            List<int>? choosedEquipmentsId = query.ChoosedEquipments;
            DateTime bookingFrom = query.BookingFrom;
            DateTime bookingTo = query.BookingTo;

            List<Room> allRooms = await _roomRepository.GetBigEnoughRoomsAsync(
                guestNumber, dogNumber, bookingFrom, bookingTo);
            List<Room> onlyRequestedRooms = new();
            if (choosedEquipmentsId is not null && choosedEquipmentsId.Count != 0)
            {
                foreach (Room room in allRooms)
                {
                    if (choosedEquipmentsId.All(x => room.Equipments.Any(e => e.Id == x)))
                    {
                        onlyRequestedRooms.Add(room);
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

    public async Task SaveOneImageAsync(SaveOneImageDTO saveOneImage)
    {
        Room? room = await _roomRepository.GetRoomByIdAsync(int.Parse(saveOneImage.RoomId));

        string[] supportedImageTypes = { "apng", "avif", "gif", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "webp" };
        string actualType = saveOneImage.Image!.FileName.Split('.')[1].ToLower();
        if (!supportedImageTypes.Contains(actualType))
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Images", "Nem támogatott kép formátum") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
        }

        if (saveOneImage.Image.Length > 2_500_000)
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Images", "A file méret max 2,5MByte") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
        };

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(saveOneImage.Image?.Name, saveOneImage.Image?.OpenReadStream()),
            PublicId = Guid.NewGuid().ToString(),
            Folder = $"Hotel/Room{saveOneImage.RoomId}",
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        var imageurl = uploadResult.SecureUrl.ToString();

        Image newImage = new()
        {
            Description = saveOneImage.Description,
            ImageUrl = imageurl,
            Room = room!
        };

        await _roomRepository.SaveOneImageAsync(newImage);
    }

    public async Task SaveMoreImageAsync(SaveMoreImageDTO saveMoreImage)
    {
        Room? room = await _roomRepository.GetRoomByIdAsync(int.Parse(saveMoreImage.RoomId));
        List<Image> images = new();

        string[] supportedImageTypes = { "apng", "avif", "gif", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "webp" };
        foreach (var actualImage in saveMoreImage.Images)
        {
            string actualType = actualImage!.FileName.Split('.')[1].ToLower();
            if (!supportedImageTypes.Contains(actualType))
            {
                List<HotelFieldError> errors = new() { new HotelFieldError("Images", "Nem támogatott kép formátum") };
                throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
            }

            if (actualImage.Length > 2_500_000)
            {
                List<HotelFieldError> errors = new() { new HotelFieldError("Images", "A file méret max 2,5MByte") };
                throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
            };
        }

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
                Room = room!
            };

            images.Add(image);
        }

        await _roomRepository.SaveMoreImageAsync(images);
    }

    private async Task DeleteImageAsync(string imageUrl)
    {
        string[] splittedUrl = imageUrl.Split('/');
        var publicId = new StringBuilder();
        publicId.Append(splittedUrl[^3]);
        publicId.Append("/");
        publicId.Append(splittedUrl[^2]);
        publicId.Append("/");
        publicId.Append(splittedUrl[^1].Split('.')[0]);
        DeletionParams dp = new(publicId.ToString());
        await _cloudinary.DestroyAsync(dp);
    }

    public async Task<RoomDetailsDTO> CreateRoomAsync(CreateRoomDTO createRoomDTO)
    {
        Room room = _mapper.Map<Room>(createRoomDTO);
        return _mapper.Map<RoomDetailsDTO>(await _roomRepository.CreateRoomAsync(room));
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _roomRepository.DeleteRoomAsync(id);
    }

    public async Task<RoomDetailsDTO> ModifyRoomAsync(RoomDetailsDTO roomDetailsDTO)
    {
        Room room = _mapper.Map<Room>(roomDetailsDTO);
        return _mapper.Map<RoomDetailsDTO>(await _roomRepository.ModifyRoomAsync(room));
    }

    public async Task DeleteImageOfRoomAsync(string url)
    {
        await _roomRepository.DeleteImageOfRoomAsync(url);
        await DeleteImageAsync(url);
    }
}