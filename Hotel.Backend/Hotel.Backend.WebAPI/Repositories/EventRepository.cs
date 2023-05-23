using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Backend.WebAPI.Repositories;

public class EventRepository : IEventRepository
{
    private readonly HotelDbContext _context;

    public EventRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(Event @event)
    {
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events
            .ToListAsync();
    }
}
