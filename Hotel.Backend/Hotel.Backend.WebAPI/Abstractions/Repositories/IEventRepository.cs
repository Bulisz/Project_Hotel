using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories
{
    public interface IEventRepository
    {
        Task<Event> CreateEventAsync(Event eventToCreate);
    }
}