using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost(nameof(CreateReservationForRoom))]
    public async Task<ActionResult<ReservationDetailsDTO>> CreateReservationForRoom(ReservationRequestDTO request)
    {
        try
        {
            ReservationDetailsDTO response = await _reservationService.CreateReservationAsync(request);
            return Ok(response);
        }
        catch (HotelException hotelException)
        {
            var error = (new { type = "hotelError", message = hotelException.Message, errors = hotelException.HotelErrors });
            return StatusCode((int)hotelException.Status, error);
        }
    }

    [HttpGet(nameof(GetAllReservations))]
    public async Task<ActionResult<IEnumerable<ReservationListItemDTO>>> GetAllReservations()
    {
            List<ReservationListItemDTO> reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<ReservationListItemDTO>>> GetMyOwnReservations(string userId)
    {
        List<ReservationListItemDTO> ownReservations = await _reservationService.GetMyOwnReservationsAsync(userId);
        return Ok(ownReservations);
    }
}
