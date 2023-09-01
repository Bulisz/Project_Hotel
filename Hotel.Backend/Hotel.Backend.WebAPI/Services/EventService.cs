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

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly Cloudinary _cloudinary;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, Cloudinary cloudinary, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _cloudinary = cloudinary;
        _mapper = mapper;
    }

    public async Task<EventDetailsDTO> CreateEventAsync(CreateEventDTO createEventDTO)
    {
        Event @event = _mapper.Map<Event>(createEventDTO);

        string[] supportedImageTypes = {"apng", "avif", "gif", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "webp" };
        string actualType = createEventDTO.Image.FileName.Split('.')[1].ToLower();
        if (!supportedImageTypes.Contains(actualType))
        {
            List<HotelFieldError> errors = new() { new HotelFieldError("Image", "Nem támogatott kép formátum") };
            throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
        }

        if (createEventDTO.Image.Length > 2_500_000)
        {
            List<HotelFieldError> errors = new(){new HotelFieldError( "Image", "A file méret max 2,5MByte")};
            throw new HotelException(HttpStatusCode.BadRequest,errors,"One or more hotelError occured");
        };

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(createEventDTO.Image?.Name, createEventDTO.Image?.OpenReadStream()),
            PublicId = Guid.NewGuid().ToString(),
            Folder = "Hotel/Event",
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        var imageurl = uploadResult.SecureUrl.ToString();
        var imageurl = uploadResult.

        @event.ImageUrl = imageurl;

        return _mapper.Map<EventDetailsDTO>(await _eventRepository.CreateEventAsync(@event));
    }

    public async Task<IEnumerable<EventDetailsDTO>> GetListOfEventsAsync()
    {
        List<Event> allEvents = await _eventRepository.GetAllEventsAsync();

        List<EventDetailsDTO> events = _mapper.Map<List<EventDetailsDTO>>(allEvents);

        return events;
    }

    public async Task<EventDetailsDTO> ModifyEventAsync(EventModifyDTO modifyEvent)
    {
        string imageUrl = string.Empty;

        if(modifyEvent.Image is not null)
        {
            await DeleteImageAsync(modifyEvent.OldImageUrl);

            string[] supportedImageTypes = { "apng", "avif", "gif", "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "svg", "webp" };
            string actualType = modifyEvent.Image.FileName.Split('.')[1].ToLower();
            if (!supportedImageTypes.Contains(actualType))
            {
                List<HotelFieldError> errors = new() { new HotelFieldError("Image", "Nem támogatott kép formátum") };
                throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
            }

            if (modifyEvent.Image.Length > 2_500_000)
            {
                List<HotelFieldError> errors = new() { new HotelFieldError("Image", "A file méret max 2,5MByte") };
                throw new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotelError occured");
            };

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(modifyEvent.Image?.Name, modifyEvent.Image?.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
                Folder = "Hotel/Event",
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            imageUrl = uploadResult.SecureUrl.ToString();
        }

        Event eventToModify = _mapper.Map<Event>(modifyEvent);
        if(modifyEvent.Image is not null)
        {
            eventToModify.ImageUrl = imageUrl;
        }
        else
        {
            eventToModify.ImageUrl = modifyEvent.OldImageUrl;
        }

        return _mapper.Map<EventDetailsDTO>(await _eventRepository.ModifyEventAsync(eventToModify));
    }

    public async Task DeleteEventAsync(int id)
    {
        Event @event = await _eventRepository.GetEventByIdAsync(id);
        DeleteImageAsync(@event.ImageUrl);
        await _eventRepository.DeleteEventAsync(@event);
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
}
