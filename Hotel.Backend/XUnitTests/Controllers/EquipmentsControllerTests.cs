using AutoFixture;
using FluentAssertions;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace XUnitTests.Controllers;

public class EquipmentsControllerTests
{
    private EquipmentsController _controller = null!;
    private readonly IEquipmentService _service = Substitute.For<IEquipmentService>();
    private readonly ILogger<EquipmentsController> _logger = Substitute.For<ILogger<EquipmentsController>>();
    private readonly IFixture _fixture = new Fixture();
    public EquipmentsControllerTests() => _controller = new EquipmentsController(_service, _logger);

    [Fact]
    public async Task CreateEquipmentTest_Valid()
    {
        //Arrange
        CreateEquipmentDTO createEquipmentDTO = _fixture.Create<CreateEquipmentDTO>();
        _service.CreateEquipmentAsync(Arg.Any<CreateEquipmentDTO>()).Returns(e => new EquipmentDTO()
        {
            Id = 1,
            IsStandard = ((CreateEquipmentDTO)e[0]).IsStandard,
            Name = ((CreateEquipmentDTO)e[0]).Name
        });

        //Act
        var result = (await _controller.CreateEquipment(createEquipmentDTO)).Result;

        //Assert
        result!.GetType().Should().Be(typeof(OkObjectResult));
        ((OkObjectResult)result!).Value!.GetType().Should().Be(typeof(EquipmentDTO));
        ((EquipmentDTO)((OkObjectResult)result!).Value!).Id.Should().Be(1);
    }
}
