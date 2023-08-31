using Castle.Components.DictionaryAdapter.Xml;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Reflection;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class EquipmentsControllerTests
{
    private EquipmentsController _controller = null!;
    private readonly Mock<IEquipmentService> _equipmentServiceMock = new();
    private readonly Mock<ILogger<EquipmentsController>> _loggerMock = new();

    [TestInitialize]
    public void Setup()
    {
        _controller = new EquipmentsController(_equipmentServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task CreateEquipmentTest_Valid()
    {
        //Arrange
        CreateEquipmentDTO createEquipmentDTO = new CreateEquipmentDTO();
        _equipmentServiceMock.Setup(s => s.CreateEquipmentAsync(It.IsAny<CreateEquipmentDTO>())).ReturnsAsync(new EquipmentDTO() { Id = 5 });

        //Act
        var result = (await _controller.CreateEquipment(createEquipmentDTO)).Result;

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.IsInstanceOfType(((OkObjectResult)result!).Value, typeof(EquipmentDTO));
        Assert.AreEqual(5, ((EquipmentDTO)((OkObjectResult)result!).Value!).Id);
    }

    [TestMethod]
    public async Task CreateEquipmentTest_HotelExeption()
    {
        //Arrange
        CreateEquipmentDTO createEquipmentDTO = new CreateEquipmentDTO();
        List<HotelFieldError> errors = new() { new HotelFieldError("name", "Ilyen néven már létezik felszereltség") };
        var ex = new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        _equipmentServiceMock.Setup(s => s.CreateEquipmentAsync(It.IsAny<CreateEquipmentDTO>())).Throws(ex);

        //Act
        var result = (await _controller.CreateEquipment(createEquipmentDTO)).Result;
        Type t = ((ObjectResult)result!).Value!.GetType();
        PropertyInfo[] pi = t.GetProperties();
        List<HotelFieldError> errorFields = (List<HotelFieldError>)pi[2].GetValue(((ObjectResult)result!).Value!)!;

        //Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(400, ((ObjectResult)result!).StatusCode);
        Assert.AreEqual("name", errorFields[0].FieldName);
        Assert.AreEqual("Ilyen néven már létezik felszereltség", errorFields[0].FieldErrorMessage);
    }

    [TestMethod]
    public async Task DeleteEquipmentTest_Valid()
    {
        //Arrange
        _equipmentServiceMock.Setup(s => s.DeleteEquipmentAsync(It.IsAny<int>()));

        //Act
        var result = await _controller.DeleteEquipment(1);

        //Assert
        Assert.IsInstanceOfType(result,typeof(OkResult));
    }

    [TestMethod]
    public async Task DeleteEquipmentTest_InValid()
    {
        //Arrange
        _equipmentServiceMock.Setup(s => s.DeleteEquipmentAsync(It.IsAny<int>())).Throws(new Exception("Hiba van"));

        //Act
        var result = await _controller.DeleteEquipment(1);

        //Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(500, ((ObjectResult)result!).StatusCode);
        Assert.AreEqual("Hiba van", ((ObjectResult)result!).Value);
    }

    [TestMethod]
    public async Task AddEquipmentToRoomTest()
    {
        //Arrange
        _equipmentServiceMock.Setup(s => s.AddEquipmentToRoomAsync(It.IsAny<EquipmentAndRoomDTO>()));

        //Act
        var result = await _controller.AddEquipmentToRoom(new EquipmentAndRoomDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task RemoveEquipmentFromRoomTest()
    {
        //Arrange
        _equipmentServiceMock.Setup(s => s.RemoveEquipmentFromRoomAsync(It.IsAny<EquipmentAndRoomDTO>()));

        //Act
        var result = await _controller.RemoveEquipmentFromRoom(new EquipmentAndRoomDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task GetStandardEquipmentsTest()
    {
        //Arrange
        IEnumerable<EquipmentDTO> equipmentList = new List<EquipmentDTO>()
        {
                new() {
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
        _equipmentServiceMock.Setup(s => s.GetStandardEquipmentsAsync()).ReturnsAsync(equipmentList);

        //Act
        var result = await _controller.GetStandardEquipments();
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (List<EquipmentDTO>)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        CollectionAssert.AreEqual(equipmentList.ToList(), resultList);
    }

    [TestMethod]
    public async Task GetNonStandardEquipmentsTest()
    {
        //Arrange
        IEnumerable<EquipmentDTO> equipmentList = new List<EquipmentDTO>()
        {
                new() {
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
        _equipmentServiceMock.Setup(s => s.GetNonStandardEquipmentsAsync()).ReturnsAsync(equipmentList);

        //Act
        var result = await _controller.GetNonStandardEquipments();
        var okResult = (OkObjectResult)result.Result!;
        var resultList = (List<EquipmentDTO>)okResult.Value!;

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        CollectionAssert.AreEqual(equipmentList.ToList(), resultList);
    }
}
