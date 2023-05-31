using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class EquipmentsController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;
    private readonly ILogger<EquipmentsController> _logger;

    public EquipmentsController(IEquipmentService equipmentService, ILogger<EquipmentsController> logger)
    {
        _equipmentService = equipmentService;
        _logger = logger;
    }

    [HttpPost(nameof(CreateEquipment))]
    public async Task<ActionResult<EquipmentDTO>> CreateEquipment(CreateEquipmentDTO createEquipmentDTO)
    {
        try
        {
            EquipmentDTO createdEquipment = await _equipmentService.CreateEquipmentAsync(createEquipmentDTO);
            return Ok(createdEquipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }

    }

    [HttpDelete("DeleteEquipment/{id}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        try
        {
            await _equipmentService.DeleteEquipmentAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut(nameof(AddEquipmentToRoom))]
    public async Task<IActionResult> AddEquipmentToRoom(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        try
        {
            await _equipmentService.AddEquipmentToRoomAsync(equipmentAndRoomDTO);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut(nameof(RemoveEquipmentFromRoom))]
    public async Task<IActionResult> RemoveEquipmentFromRoom(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        try
        {
            await _equipmentService.RemoveEquipmentFromRoomAsync(equipmentAndRoomDTO);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet(nameof(GetStandardEquipments))]
    public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetStandardEquipments()
    {
        try
        {
            IEnumerable<EquipmentDTO> standardEquipments = await _equipmentService.GetStandardEquipmentsAsync();
            return Ok(standardEquipments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }


    [HttpGet(nameof(GetNonStandardEquipments))]
    public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetNonStandardEquipments()
    {
        try
        {
            IEnumerable<EquipmentDTO> nonStandardEquipments = await _equipmentService.GetNonStandardEquipmentsAsync();
            return Ok(nonStandardEquipments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
