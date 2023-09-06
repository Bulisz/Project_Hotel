using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using NSubstitute;

namespace XUnitTests.Services;

public class EquipmentServiceTests
{
    private EquipmentService _service = null!;
    private readonly IEquipmentRepository _repository = Substitute.For<IEquipmentRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IFixture _fixture = new Fixture();

    public EquipmentServiceTests() => _service = new EquipmentService(_repository, _mapper);

    [Fact]
    public async Task CreateEquipmentTest()
    {
        //Arrange
        CreateEquipmentDTO createEquipmentDTO = _fixture.Create<CreateEquipmentDTO>();

        _mapper.Map<Equipment>(Arg.Any<CreateEquipmentDTO>()).Returns(e => new Equipment()
        {
            Id = 1,
            Name = ((CreateEquipmentDTO)e[0]).Name,
            IsStandard = ((CreateEquipmentDTO)e[0]).IsStandard
        });
        _repository.CreateEquipmentAsync(Arg.Any<Equipment>()).Returns(e => Task.FromResult((Equipment)e[0]));
        _mapper.Map<EquipmentDTO>(Arg.Any<Equipment>()).Returns(e => new EquipmentDTO()
        {
            Id = ((Equipment)e[0]).Id,
            Name = ((Equipment)e[0]).Name,
            IsStandard = ((Equipment)e[0]).IsStandard
        });
        
        //Act
        var result = await _service.CreateEquipmentAsync(createEquipmentDTO);

        //Assert
        result.Name.Should().Be(createEquipmentDTO.Name);
        result.Id.Should().Be(1);
        result.IsStandard.Should().Be(createEquipmentDTO.IsStandard);
    }
}
