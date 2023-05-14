using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }


    [HttpGet(nameof(GetListOfRooms))]
    public async Task<ActionResult<IEnumerable<RoomListDTO>>> GetListOfRooms()
    {
        var listedRooms = await _roomService.GetListOfRoomsAsync();
        return Ok(listedRooms);
    }

    [HttpGet("GetRoomById/{id}")]
    public async Task<ActionResult<RoomDetailsDTO>> GetRoomById(int id)
    {
         try
        {
            RoomDetailsDTO roomDetails = await _roomService.GetRoomByIdAsync(id);
            return Ok(roomDetails);
        }
        catch (HotelException ex)
        {
            var error = (new { type = "hotelError", message = ex.Message, errors = ex.HotelErrors });
            return StatusCode((int)ex.Status, error);
        }
    }

    [HttpGet(nameof(GetAvailableRooms))]
    public async Task<ActionResult<IEnumerable<RoomListDTO>>> GetAvailableRooms()
    {
        var listedRooms = await _roomService.GetAvailableRoomsAsync();
        return Ok(listedRooms);
    }
}
