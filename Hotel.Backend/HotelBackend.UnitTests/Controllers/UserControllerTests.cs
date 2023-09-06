using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Reflection;
using System.Security.Claims;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class UserControllerTests
{
    private UsersController _controller = null!;
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<ILogger<UsersController>> _loggerMock = new();
    private readonly Mock<IOptions<AppSettings>> _optionsMock = new();

    [TestInitialize]
    public void Setup()
    {
        _controller = new UsersController(_optionsMock.Object, _jwtServiceMock.Object, _userServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetUserByNameTest_Valid()
    {
        //Arrange
        _userServiceMock.Setup(s => s.GetUserByNameAsync(It.IsAny<string>())).Returns<string>(u => Task.FromResult(new UserDetails() { UserName = u })!);

        //Act
        var result = await _controller.GetUserByName("Béla");

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual("Béla",((UserDetails)((OkObjectResult)result.Result!).Value!).UserName);
    }

    [TestMethod]
    public async Task GetUserByNameTest_NoContent()
    {
        //Arrange
        _userServiceMock.Setup(s => s.GetUserByNameAsync(It.IsAny<string>()));

        //Act
        var result = await _controller.GetUserByName("NoName");

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task GetUserByNameTest_HotelException()
    {
        //Arrange
        List<HotelFieldError> errors = new() { new HotelFieldError("userName", "Invalid Username") };
        var ex = new HotelException(HttpStatusCode.BadRequest, errors, "One or more hotel errors occurred.");
        _userServiceMock.Setup(s => s.GetUserByNameAsync(It.IsAny<string>())).Throws(ex);

        //Act
        var result = (await _controller.GetUserByName("NoName")).Result;
        Type t = ((ObjectResult)result!).Value!.GetType();
        PropertyInfo[] pi = t.GetProperties();
        List<HotelFieldError> errorFields = (List<HotelFieldError>)pi[2].GetValue(((ObjectResult)result!).Value!)!;

        //Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(400, ((ObjectResult)result!).StatusCode);
        Assert.AreEqual("userName", errorFields[0].FieldName);
        Assert.AreEqual("Invalid Username", errorFields[0].FieldErrorMessage);
    }

    [TestMethod]
    public async Task GetUserByNameTest_Exception()
    {
        //Arrange
        _userServiceMock.Setup(s => s.GetUserByNameAsync(It.IsAny<string>())).Throws(new Exception("Hiba van"));

        //Act
        var result = (await _controller.GetUserByName("NoName")).Result;

        //Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(500, ((ObjectResult)result!).StatusCode);
        Assert.AreEqual("Hiba van", ((ObjectResult)result!).Value);
    }

    [TestMethod]
    public async Task GetUserByIdTest()
    {
        //Arrange
        _userServiceMock.Setup(s => s.GetUserByIdAsync(It.IsAny<string>())).Returns<string>(u => Task.FromResult(new UserDetails() { Id = u })!);

        //Act
        var result = await _controller.GetUserById("R2D2");

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual("R2D2", ((UserDetails)((OkObjectResult)result.Result!).Value!).Id);
    }

    
}
