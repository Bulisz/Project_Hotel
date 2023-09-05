using AutoFixture;
using FluentAssertions;
using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace XUnitTests.Repositories;

public class EquipmentRepositoryTest : IDisposable
{
    private HotelDbContext _dbContext = null!;
    private EquipmentRepository _repository = null!;
    private readonly IFixture _fixture = new Fixture();
    public EquipmentRepositoryTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlite("Data Source = :memory:");
        _dbContext = new HotelDbContext(optionsBuilder.Options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();
        _repository = new EquipmentRepository(_dbContext);
    }

    [Fact]
    public async Task CreateEquipmentTest_Valid()
    {
        //Arrange
        Equipment equipment = _fixture.Build<Equipment>().Without(e => e.Rooms).Create();

        //Act
        var result = await _repository.CreateEquipmentAsync(equipment);

        //Assert
        result.Should().Be(equipment);
        _dbContext.Equipments.Count().Should().Be(1);
    }

    [Fact]
    public async Task CreateEquipmentTest_AlreadyExist()
    {
        //Arrange
        Equipment equipment = _fixture.Build<Equipment>().Without(e => e.Rooms).Create();
        _dbContext.Add(equipment);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = () => _repository.CreateEquipmentAsync(equipment);

        //Assert
        await result.Should().ThrowAsync<HotelException>().WithMessage("One or more hotel errors occurred.");
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
