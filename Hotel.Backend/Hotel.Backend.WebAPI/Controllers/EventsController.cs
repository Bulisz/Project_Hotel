using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost("CreateEvent")]
    public async Task<ActionResult<EventDetailsDTO>> CreateEvent(CreateEventDTO createEvent)
    {
        EventDetailsDTO createdEvent = await _eventService.CreateEventAsync(createEvent);
        return Ok(createEvent);
    }
}
