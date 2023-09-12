using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }


    [HttpGet(nameof(GetListOfRooms))]
    public async Task<ActionResult<IEnumerable<RoomListDTO>>> GetListOfRooms()
    {
        try
        {
            var listedRooms = await _roomService.GetListOfRoomsAsync();
            return Ok(listedRooms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
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

    [HttpGet(nameof(GetAvailableRooms))]
    public async Task<ActionResult<IEnumerable<RoomListDTO>>> GetAvailableRooms([FromQuery] RoomSelectorDTO query)
    {
        try
        {
            var listedRooms = await _roomService.GetAvailableRoomsAsync(query);
            return Ok(listedRooms);
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

    [Authorize(Roles = "Admin")]
    [HttpPost(nameof(CreateRoom))]
    public async Task<ActionResult<RoomDetailsDTO>> CreateRoom(CreateRoomDTO createRoomDTO)
    {
        try
        {
            RoomDetailsDTO roomDetailsDTO = await _roomService.CreateRoomAsync(createRoomDTO);
            return Ok(roomDetailsDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("SaveOneImage")]
    public async Task<ActionResult> SaveOneImage([FromForm] SaveOneImageDTO saveOneImage)
    {
        try
        {
            await _roomService.SaveOneImageAsync(saveOneImage);
            return Ok();
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

    [Authorize(Roles = "Admin")]
    [HttpPost("SaveMoreImage")]
    public async Task<ActionResult> SaveMoreImage([FromForm] SaveMoreImageDTO saveMoreImage)
    {
        try
        {
            await _roomService.SaveMoreImageAsync(saveMoreImage);
            return Ok();
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

    [Authorize(Roles = "Admin")]
    [HttpDelete(nameof(DeleteImageOfRoom))]
    public async Task<ActionResult> DeleteImageOfRoom(DeleteImageDTO image)
    {
        try
        {
            await _roomService.DeleteImageOfRoomAsync(image.ImageUrl);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteRoom/{id}")]
    public async Task<ActionResult> DeleteRoom(int id)
    {
        try
        {
            await _roomService.DeleteRoomAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut(nameof(ModifyRoom))]
    public async Task<ActionResult<RoomDetailsDTO>> ModifyRoom(RoomDetailsDTO roomDetailsDTO)
    {
        try
        {
            RoomDetailsDTO modifiedRoom = await _roomService.ModifyRoomAsync(roomDetailsDTO);
            return Ok(modifiedRoom);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
