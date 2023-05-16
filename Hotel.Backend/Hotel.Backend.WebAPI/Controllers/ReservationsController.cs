using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers
{
    [Route("hotel/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private ReservationService _reservationService;

        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDetailsDTO>> createReservationForRoom(ReservationRequestDTO request)
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


     
    }
}
