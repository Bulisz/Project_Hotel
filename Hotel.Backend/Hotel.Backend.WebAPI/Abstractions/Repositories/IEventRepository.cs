using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories
{
    public interface IEventRepository
    {
        Task<Event> CreateEventAsync(Event eventToCreate);
        Task DeleteEventAsync(Event @event);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> ModifyEventAsync(Event eventToModify);
    }
}