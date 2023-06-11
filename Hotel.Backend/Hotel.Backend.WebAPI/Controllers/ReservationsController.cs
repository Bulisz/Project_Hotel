using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ICalendarService _calendarService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(IReservationService reservationService, ICalendarService calendarService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _calendarService = calendarService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost(nameof(CreateReservationForRoom))]
    public async Task<ActionResult<ReservationDetailsDTO>> CreateReservationForRoom(ReservationRequestDTO request)
    {
        try
        {
            ReservationDetailsDTO response = await _reservationService.CreateReservationAsync(request);
            return Ok(response);
        }
        catch (HotelException ex)
        {
            _logger.LogError(ex, ex.Message);
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Operator")]
    [HttpGet(nameof(GetAllReservations))]
    public async Task<ActionResult<IEnumerable<ReservationListItemDTO>>> GetAllReservations()
    {
        try
        {
            List<ReservationListItemDTO> reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<ReservationListItemDTO>>> GetMyOwnReservations(string userId)
    {
        try
        {
            List<ReservationListItemDTO> ownReservations = await _reservationService.GetMyOwnReservationsAsync(userId);
            return Ok(ownReservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{reservationId}")]
    public async Task<ActionResult> DeleteReservation(int reservationId)
    {
        try
        {
            await _reservationService.DeleteReservationAsync(reservationId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Operator")]
    [HttpGet("GetThisMonthCalendar/{year}/{month}")]
    public async Task<ActionResult<List<ThisMonthCalendarDTO>>> GetThisMonthCalendar(int year, int month)
    {
        try
        {
            List<ThisMonthCalendarDTO> calendar = await _calendarService.GetAllDaysOfMonthAsync(year, month);
            return Ok(calendar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
