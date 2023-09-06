using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IEventService
{
    Task<EventDetailsDTO> CreateEventAsync(CreateEventDTO createEventDTO);
    Task DeleteEventAsync(int id);
    Task<IEnumerable<EventDetailsDTO>> GetListOfEventsAsync();
    Task<EventDetailsDTO> ModifyEventAsync(EventModifyDTO modifyEvent);
}