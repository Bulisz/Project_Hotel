using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.UnitTests.Repositories;

[TestClass]
public class EquipmentRepositoryTests
{
    private HotelDbContext _dbContext = null!;
    private EquipmentRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlite("Data Source = :memory:");
        _dbContext = new HotelDbContext(optionsBuilder.Options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _repository = new EquipmentRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task CreateEquipmentTest_Valid()
    {
        //Arrange
        Equipment equipment = new()
        {
            IsStandard = true,
            Name = "Test"
        };

        //Act
        var createdEquipment = await _repository.CreateEquipmentAsync(equipment);

        //Assert
        Assert.AreEqual(equipment, createdEquipment);
        Assert.AreEqual(1, _dbContext.Equipments.Count());
    }

    [TestMethod]
    public async Task CreateEquipmentTest_AlreadyExist()
    {
        //Arrange
        Equipment equipment = new()
        {
            IsStandard = true,
            Name = "Test"
        };
        _dbContext.Add(equipment);
        await _dbContext.SaveChangesAsync();

        //Act & Assert
        _ = Assert.ThrowsExceptionAsync<HotelException>(() => _repository.CreateEquipmentAsync(equipment));
    }

    [TestMethod]
    public async Task DeleteEquipmentTest()
    {
        //Arrange
        Equipment equipment = new()
        {
            IsStandard = true,
            Name = "Test"
        };
        _dbContext.Add(equipment);
        await _dbContext.SaveChangesAsync();

        //Act
        await _repository.DeleteEquipmentAsync(1);

        //Assert
        Assert.AreEqual(0, _dbContext.Equipments.Count());
    }

    [TestMethod]
    public async Task GetNonStandardEquipmentTest()
    {
        //Arrange
        List<Equipment> equipmentList = new()
        {
                new(){
                    IsStandard = true,
                    Name = "Test2"
                },
                new(){
                    IsStandard = false,
                    Name = "Test1"
                },
                new(){
                    IsStandard = true,
                    Name = "Test0"
                },
        };
        _dbContext.AddRange(equipmentList);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _repository.GetNonStandardEquipmentAsync();

        //Assert
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public async Task GetStandardEquipmentTest()
    {
        //Arrange
        List<Equipment> equipmentList = new()
        {
                new(){
                    IsStandard = true,
                    Name = "Test2"
                },
                new(){
                    IsStandard = false,
                    Name = "Test1"
                },
                new(){
                    IsStandard = true,
                    Name = "Test0"
                },
        };
        _dbContext.AddRange(equipmentList);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _repository.GetStandardEquipmentAsync();

        //Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public async Task AddEquipmentToRoomTest()
    {
        //Arrange
        Equipment equipment = new()
        {
            IsStandard = true,
            Name = "Test"
        };
        Room room = new Room()
        {
            Name = "Room",
            Description = "TestRoom",
        };
        _dbContext.Add(equipment);
        _dbContext.Add(room);
        await _dbContext.SaveChangesAsync();
        EquipmentAndRoomDTO equ = new()
        {
            RoomId = 1,
            EquipmentId = 1,
        };

        //Act
        await _repository.AddEquipmentToRoomAsync(equ);

        //Assert
        Assert.AreEqual("Test", room.Equipments.ToList()[0].Name);
    }

    [TestMethod]
    public async Task RemoveEquipmentFromRoomTest()
    {
        //Arrange
        Equipment equipment = new()
        {
            IsStandard = true,
            Name = "Test"
        };
        Room room = new Room()
        {
            Name = "Room",
            Description = "TestRoom",
            Equipments = new List<Equipment> { equipment }
        };
        _dbContext.Add(room);
        await _dbContext.SaveChangesAsync();
        EquipmentAndRoomDTO equ = new()
        {
            RoomId = 1,
            EquipmentId = 1,
        };

        //Act
        await _repository.RemoveEquipmentFromRoomAsync(equ);

        //Assert
        Assert.AreEqual(0, room.Equipments.Count());
    }
}
