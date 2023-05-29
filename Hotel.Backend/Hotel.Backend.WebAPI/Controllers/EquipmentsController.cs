using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class EquipmentsController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentsController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpPut(nameof(CreateEquipment))]
    public async Task<ActionResult<EquipmentDTO>> CreateEquipment(CreateEquipmentDTO createEquipmentDTO)
    {
        EquipmentDTO createdEquipment = await _equipmentService.CreateEquipmentAsync(createEquipmentDTO);
        return Ok(createdEquipment);
    }

    [HttpDelete("DeleteEquipment/{id}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        await _equipmentService.DeleteEquipmentAsync(id);
        return Ok();
    }

    [HttpPut(nameof(AddEquipmentToRoom))]
    public async Task<IActionResult> AddEquipmentToRoom(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        await _equipmentService.AddEquipmentToRoomAsync(equipmentAndRoomDTO);
        return Ok();
    }

    [HttpPut(nameof(RemoveEquipmentFromRoom))]
    public async Task<IActionResult> RemoveEquipmentFromRoom(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        await _equipmentService.RemoveEquipmentFromRoomAsync(equipmentAndRoomDTO);
        return Ok();
    }

    [HttpGet(nameof(GetStandardEquipments))]
    public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetStandardEquipments()
    {
        IEnumerable<EquipmentDTO> standardEquipments = await _equipmentService.GetStandardEquipmentsAsync();
        return Ok(standardEquipments);
    }


    [HttpGet(nameof(GetNonStandardEquipments))]
    public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetNonStandardEquipments()
    {
        IEnumerable<EquipmentDTO> nonStandardEquipments = await _equipmentService.GetNonStandardEquipmentsAsync();
        return Ok(nonStandardEquipments);
    }
}
