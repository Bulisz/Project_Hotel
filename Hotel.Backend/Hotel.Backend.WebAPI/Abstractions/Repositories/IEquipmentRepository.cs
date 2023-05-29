using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Repositories;

public interface IEquipmentRepository
{
    Task<Equipment> CreateEquipmentAsync(Equipment equipment);
    Task DeleteEquipmentAsync(int id);
    Task<IEnumerable<Equipment>> GetStandardEquipmentAsync();
    Task AddEquipmentToRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO);
    Task RemoveEquipmentFromRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO);
    Task<IEnumerable<Equipment>> GetNonStandardEquipmentAsync();
}