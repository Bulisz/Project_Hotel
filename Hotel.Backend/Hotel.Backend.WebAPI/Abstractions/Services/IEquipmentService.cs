using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services;

public interface IEquipmentService
{
    Task<EquipmentDTO> CreateEquipmentAsync(CreateEquipmentDTO createEquipmentDTO);
    Task DeleteEquipmentAsync(int id);
    Task<IEnumerable<EquipmentDTO>> GetStandardEquipmentsAsync();
    Task<IEnumerable<EquipmentDTO>> GetNonStandardEquipmentsAsync();
    Task AddEquipmentToRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO);
    Task RemoveEquipmentFromRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO);

}