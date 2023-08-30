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

        //Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(400, ((ObjectResult)result!).StatusCode);
    }
}
