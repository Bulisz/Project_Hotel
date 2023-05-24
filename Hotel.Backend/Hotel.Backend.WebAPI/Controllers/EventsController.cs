using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
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
    public async Task<ActionResult<EventDetailsDTO>> CreateEvent([FromForm]CreateEventDTO createEvent)
    {
        try
        {
            EventDetailsDTO createdEvent = await _eventService.CreateEventAsync(createEvent);
            return Ok(createEvent);
        }
        catch (HotelException ex)
        {
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
    }

    [HttpGet(nameof(GetListOfEvents))]
    public async Task<ActionResult<IEnumerable<EventDetailsDTO>>> GetListOfEvents()
    {
        var listedRooms = await _eventService.GetListOfEventsAsync();
        return Ok(listedRooms);
    }

    [HttpPut(nameof(ModifyEvent))]
    public async Task<ActionResult<EventDetailsDTO>> ModifyEvent([FromForm] EventModifyDTO modifyEvent)
    {
        try
        {

            EventDetailsDTO eventDetailsDTO = await _eventService.ModifyEventAsync(modifyEvent);
            return Ok(eventDetailsDTO);
        }
        catch (HotelException ex)
        {
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
    }

    [HttpDelete("DeleteEvent/{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        await _eventService.DeleteEventAsync(id);
        return Ok();
    }
}   
