using AutoMapper;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Moq;

namespace HotelBackend.UnitTests.Services;

[TestClass]
public class EquipmentServiceTests
{
    private EquipmentService _service = null!;
    private readonly Mock<IEquipmentRepository> _equipmentReposítoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [TestInitialize]
    public void Setup()
    {
        _service = new EquipmentService(_equipmentReposítoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task CreateEquipmentAsyncTest()
    {
        //Arrange
        CreateEquipmentDTO createEquipmentDTO = new();
        Equipment equipment = new()
        {
            Id = 1,
            IsStandard = true,
            Name = "Test",
        };

        _mapperMock.Setup(m => m.Map<Equipment>(It.IsAny<CreateEquipmentDTO>())).Returns(equipment);
        _equipmentReposítoryMock.Setup(r => r.CreateEquipmentAsync(It.IsAny<Equipment>())).Returns<Equipment>(e => Task.FromResult(e));
        _mapperMock.Setup(m => m.Map<EquipmentDTO>(It.IsAny<Equipment>())).Returns<Equipment>(e => new EquipmentDTO()
        {
            Id= e.Id,
            Name= e.Name,
            IsStandard = e.IsStandard,
        });

        //Act
        var result = await _service.CreateEquipmentAsync(createEquipmentDTO);

        //Assert
        Assert.AreEqual("Test", result.Name);
        Assert.AreEqual(true, result.IsStandard);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public async Task GetStandardEquipmentsTest()
    {
        //Arrange
        List<Equipment> equipmentList = new()
        {
                new(){
                    IsStandard = true,
                    Name = "Test2"
                },
                new(){
                    IsStandard = true,
                    Name = "Test1"
                },
                new(){
                    IsStandard = true,
                    Name = "Test0"
                },
        };

        _equipmentReposítoryMock.Setup(r => r.GetStandardEquipmentAsync()).ReturnsAsync(equipmentList);
        _mapperMock.Setup(m => m.Map<List<EquipmentDTO>>(It.IsAny<List<Equipment>>())).Returns<List<Equipment>>(e => e.Select(x =>  new EquipmentDTO()
        {
            Id = x.Id,
            Name = x.Name,
            IsStandard = x.IsStandard,
        }).ToList());

        //Act
        var result = await _service.GetStandardEquipmentsAsync();

        //Assert
        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public async Task GetNonStandardEquipmentsTest()
    {
        //Arrange
        List<Equipment> equipmentList = new()
        {
                new(){
                    IsStandard = false,
                    Name = "Test2"
                },
                new(){
                    IsStandard = false,
                    Name = "Test1"
                },
                new(){
                    IsStandard = false,
                    Name = "Test0"
                },
        };

        _equipmentReposítoryMock.Setup(r => r.GetNonStandardEquipmentAsync()).ReturnsAsync(equipmentList);
        _mapperMock.Setup(m => m.Map<List<EquipmentDTO>>(It.IsAny<List<Equipment>>())).Returns<List<Equipment>>(e => e.Select(x => new EquipmentDTO()
        {
            Id = x.Id,
            Name = x.Name,
            IsStandard = x.IsStandard,
        }).ToList());

        //Act
        var result = await _service.GetNonStandardEquipmentsAsync();

        //Assert
        Assert.AreEqual(3, result.Count());
    }

}
