using AutoMapper;
using CloudinaryDotNet;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Migrations;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Moq;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelBackend.UnitTests.Services;

[TestClass]
public class RoomServiceTests
{
    private RoomService _roomService = null!;
    private readonly Mock<IRoomRepository> _roomRepositoryMock = new Mock<IRoomRepository>();
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<Cloudinary> _cloudinaryMock = new Mock<Cloudinary>("cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name");

    [TestInitialize]
    public void Setup()
    {
        _roomService = new RoomService(
                _roomRepositoryMock.Object,
                _mapperMock.Object,
                _cloudinaryMock.Object
                );
    }

    [DataTestMethod]
    [DataRow(1, 2, 1)]
    [DataRow(4, 5, 2)]
    [DataRow(1, 6, 0)]
    public async Task GetAvailableRoomsAsyncTest_withValidInqury(int choosedEq1, int choosedEq2, int expectedResult)
    {
        // Arrange
        var query = new RoomSelectorDTO
        {
            NumberOfBeds = 1,
            MaxNumberOfDogs = 1,
            BookingFrom = new DateTime(2023, 1, 1),
            BookingTo = new DateTime(2023, 1, 4),
            ChoosedEquipments = new List<int> { choosedEq1, choosedEq2 }
        };
        Equipment eq1 = new Equipment { Id = 1 };
        Equipment eq2 = new Equipment { Id = 2 };
        Equipment eq3 = new Equipment { Id = 3 };
        Equipment eq4 = new Equipment { Id = 4 };
        Equipment eq5 = new Equipment { Id = 5 };
        Equipment eq6 = new Equipment { Id = 6 };

        var rooms = new List<Room>
            {
                new Room { Id = 1, Available = true, Equipments = new List<Equipment> {eq1, eq2, eq3 }},
                new Room { Id = 2, Available = true, Equipments = new List<Equipment> {eq1, eq4, eq5 }},
                new Room { Id = 3, Available = true, Equipments = new List<Equipment> {eq2, eq3, eq5 }},
                new Room { Id = 4, Available = true, Equipments = new List<Equipment> {eq3, eq4, eq5 }}
            };

        _roomRepositoryMock.Setup(m => m.GetBigEnoughRoomsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .ReturnsAsync(rooms);
        _mapperMock.Setup(m => m.Map<List<RoomListDTO>>(It.IsAny<List<Room>>()))
            .Returns<List<Room>>(p => p.Select(x => new RoomListDTO()
        {
            Id = x.Id,
        }).ToList());

        // Act
        var result = await _roomService.GetAvailableRoomsAsync(query);

        // Assert
        Assert.AreEqual(expectedResult, result.ToList().Count);
    }

    [TestMethod]

    public async Task GetListOfRoomsAsyncTest()
    {
        // Arrange
        var allRooms = new List<Room> 
        {
            new Room { Id = 1 }, 
            new Room { Id = 2 }
        };

        _roomRepositoryMock.Setup(m => m.GetAllRoomsAsync()).ReturnsAsync(allRooms);
        _mapperMock.Setup(m => m.Map<List<RoomListDTO>>(It.IsAny<List<Room>>()))
            .Returns<List<Room>>(p => p.Select(x => new RoomListDTO()
        {
                Id = x.Id,
        }).ToList());

        // Act
        var result = await _roomService.GetListOfRoomsAsync();

        // Assert
        Assert.AreEqual(2, result.ToList().Count);
        Assert.AreEqual(1, result.ToList()[0].Id);
        Assert.AreEqual(2, result.ToList()[1].Id);
    }
}
