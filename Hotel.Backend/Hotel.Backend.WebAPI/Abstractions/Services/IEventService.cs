using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IEventService
    {
        Task<EventDetailsDTO> CreateEventAsync(CreateEventDTO createEventDTO);
        Task<IEnumerable<EventDetailsDTO>> GetListOfEventsAsync();
    }
}