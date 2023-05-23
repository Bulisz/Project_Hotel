using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
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

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(createEventDTO.Image?.Name, createEventDTO.Image?.OpenReadStream()),
            PublicId = Guid.NewGuid().ToString(),
            Folder = "Hotel/Event",
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        var imageurl = uploadResult.SecureUrl.ToString();

        @event.ImageUrl = imageurl;

        return _mapper.Map<EventDetailsDTO>(await _eventRepository.CreateEventAsync(@event));
    }

    public async Task<IEnumerable<EventDetailsDTO>> GetListOfEventsAsync()
    {
        List<Event> allEvents = await _eventRepository.GetAllEventsAsync();

        List<EventDetailsDTO> events = _mapper.Map<List<EventDetailsDTO>>(allEvents);

        return events;
    }
}
