using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class EmailControllerTests
{
    private EmailsController _controller = null!;
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<EmailsController>> _loggerMock = new();

    [TestInitialize]
    public void Setup()
    {
        _controller = new EmailsController(_emailServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task SendEmailTest_Valid()
    {
        //Arrange
        _emailServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<EmailDTO>()));

        //Act
        var result = await _controller.SendEmail(new EmailDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task SendEmailTest_Exception()
    {
        //Arrange
        _emailServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<EmailDTO>())).Throws(new Exception("Hiba van"));

        //Act
        var result = await _controller.SendEmail(new EmailDTO());

        //Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        Assert.AreEqual(500, ((ObjectResult)result!).StatusCode);
        Assert.AreEqual("Hiba van", ((ObjectResult)result!).Value);
    }
}
