using AutoMapper;
using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public EquipmentService(IEquipmentRepository equipmentRepository, IMapper mapper, Cloudinary cloudinary)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentDTO> CreateEquipmentAsync(CreateEquipmentDTO createEquipmentDTO)
    {
        Equipment equipment = _mapper.Map<Equipment>(createEquipmentDTO);
        return _mapper.Map<EquipmentDTO>(await _equipmentRepository.CreateEquipmentAsync(equipment));
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        await _equipmentRepository.DeleteEquipmentAsync(id);
    }

    public async Task<IEnumerable<EquipmentDTO>> GetStandardEquipmentsAsync()
    {
        IEnumerable<Equipment> standardEquipment = await _equipmentRepository.GetStandardEquipmentAsync();
        List<EquipmentDTO> standardEquipmentDTO = _mapper.Map<List<EquipmentDTO>>(standardEquipment);
        return standardEquipmentDTO;
    }

    public async Task<IEnumerable<EquipmentDTO>> GetNonStandardEquipmentsAsync()
    {
        IEnumerable<Equipment> nonStandardEquipment = await _equipmentRepository.GetNonStandardEquipmentAsync();
        List<EquipmentDTO> nonStandardEquipmentDTO = _mapper.Map<List<EquipmentDTO>>(nonStandardEquipment);
        return nonStandardEquipmentDTO;
    }

    public async Task AddEquipmentToRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        await _equipmentRepository.AddEquipmentToRoomAsync(equipmentAndRoomDTO);
    }

    public async Task RemoveEquipmentFromRoomAsync(EquipmentAndRoomDTO equipmentAndRoomDTO)
    {
        await _equipmentRepository.RemoveEquipmentFromRoomAsync(equipmentAndRoomDTO);
    }
}
