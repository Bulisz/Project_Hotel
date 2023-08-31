using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class RoomControllerTests
{
    private RoomsController _controller = null!;
    private readonly Mock<IRoomService> _roomServiceMock = new();
    private readonly Mock<ILogger<RoomsController>> _loggerMock = new();

    [TestInitialize]
    public void Setup()
    {
        _controller = new RoomsController(_roomServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetListOfRoomsTest()
    {
        //Arrange
        IEnumerable<RoomListDTO> roomList = new List<RoomListDTO>()
        {
                new() {
                    Name = "Test2"
                },
                new(){
                    Name = "Test1"
                },
                new(){
                    Name = "Test0"
                },
        };
        _roomServiceMock.Setup(s => s.GetListOfRoomsAsync()).ReturnsAsync(roomList);

        //Act
        var result = await _controller.GetListOfRooms();
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (List<RoomListDTO>)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        CollectionAssert.AreEqual(roomList.ToList(), resultList);
    }

    [TestMethod]
    public async Task GetRoomByIdTest_Valid()
    {
        RoomDetailsDTO room = new() { Name = "test" };

        _roomServiceMock.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>())).ReturnsAsync(room);

        //Act
        var result = await _controller.GetRoomById(1);
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (RoomDetailsDTO)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(room.Name, resultList.Name);
    }

    [TestMethod]
    public async Task GetRoomByIdTest_HotelException()
    {
        List<HotelFieldError> errors = new() { new HotelFieldError("Id", "Room id is invalid") };
        var ex = new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");

        _roomServiceMock.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>())).Throws(ex);

        //Act
        var result = await _controller.GetRoomById(1);
        var okResult = (ObjectResult)result.Result!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
        Assert.AreEqual(400, okResult.StatusCode);
    }

    [TestMethod]
    public async Task GetAvailableRoomsTest()
    {
        //Arrange
        IEnumerable<RoomListDTO> roomList = new List<RoomListDTO>()
        {
                new() {
                    Name = "Test2"
                },
                new(){
                    Name = "Test1"
                },
                new(){
                    Name = "Test0"
                },
        };
        _roomServiceMock.Setup(s => s.GetAvailableRoomsAsync(It.IsAny<RoomSelectorDTO>())).ReturnsAsync(roomList);

        //Act
        var result = await _controller.GetAvailableRooms(new RoomSelectorDTO());
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (List<RoomListDTO>)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        CollectionAssert.AreEqual(roomList.ToList(), resultList);
    }

    [TestMethod]
    public async Task CreateRoomTest()
    {
        RoomDetailsDTO room = new() { Name = "test" };

        _roomServiceMock.Setup(s => s.CreateRoomAsync(It.IsAny<CreateRoomDTO>())).ReturnsAsync(room);

        //Act
        var result = await _controller.CreateRoom(new CreateRoomDTO());
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (RoomDetailsDTO)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(room.Name, resultList.Name);
    }

    [TestMethod]
    public async Task SaveOneImageTest()
    {
        //Arrange
        _roomServiceMock.Setup(s => s.SaveOneImageAsync(It.IsAny<SaveOneImageDTO>()));

        //Act
        var result = await _controller.SaveOneImage(new SaveOneImageDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task SaveMoreImageTest()
    {
        //Arrange
        _roomServiceMock.Setup(s => s.SaveMoreImageAsync(It.IsAny<SaveMoreImageDTO>()));

        //Act
        var result = await _controller.SaveMoreImage(new SaveMoreImageDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task DeleteImageOfRoomTest()
    {
        //Arrange
        _roomServiceMock.Setup(s => s.DeleteImageOfRoomAsync(It.IsAny<string>()));

        //Act
        var result = await _controller.DeleteImageOfRoom(new DeleteImageDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task DeleteRoomTest()
    {
        //Arrange
        _roomServiceMock.Setup(s => s.DeleteRoomAsync(It.IsAny<int>()));

        //Act
        var result = await _controller.DeleteRoom(1);

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task ModifyRoomTest()
    {
        RoomDetailsDTO room = new() { Name = "test" };

        _roomServiceMock.Setup(s => s.ModifyRoomAsync(It.IsAny<RoomDetailsDTO>())).ReturnsAsync(room);

        //Act
        var result = await _controller.ModifyRoom(new RoomDetailsDTO());
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (RoomDetailsDTO)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(room.Name, resultList.Name);
    }
}
