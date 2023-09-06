using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IEventService eventService, ILogger<EventsController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin,Operator")]
    [HttpPost("CreateEvent")]
    public async Task<ActionResult<EventDetailsDTO>> CreateEvent([FromForm] CreateEventDTO createEvent)
    {
        try
        {
            EventDetailsDTO createdEvent = await _eventService.CreateEventAsync(createEvent);
            return Ok(createdEvent);
        }
        catch (HotelException ex)
        {
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet(nameof(GetListOfEvents))]
    public async Task<ActionResult<IEnumerable<EventDetailsDTO>>> GetListOfEvents()
    {
        try
        {
            var listedEvents = await _eventService.GetListOfEventsAsync();
            return Ok(listedEvents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Operator")]
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
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Operator")]
    [HttpDelete("DeleteEvent/{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        try
        {
            await _eventService.DeleteEventAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
