using AutoFixture;
using AutoMapper;
using CloudinaryDotNet;
using FluentAssertions;
using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using NSubstitute;
using System.Diagnostics;

namespace XUnitTests.Services;

public class RoomServiceTests
{
    private RoomService _roomService = null!;
    private readonly IRoomRepository _roomRepository = Substitute.For<IRoomRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly Cloudinary _cloudinary = Substitute.For<Cloudinary>("cloudinary://123456789012345:abcdefghijklmnopqrstuvwxyzA@cloud_name");
    private readonly IFixture _fixture = new Fixture();

    public RoomServiceTests() => _roomService = new RoomService(_roomRepository, _mapper, _cloudinary);

    [Theory]
    [ClassData(typeof(EquipmentTestData))]
    public async Task GetAvailableRoomsAsyncTest_withValidInqury(int choosedEq1, int choosedEq2, int expectedResult)
    {
        //Arrange
        var query = new RoomSelectorDTO
        {
            NumberOfBeds = 1,
            MaxNumberOfDogs = 1,
            BookingFrom = new DateTime(2023, 1, 1),
            BookingTo = new DateTime(2023, 1, 4),
            ChoosedEquipments = new List<int> { choosedEq1, choosedEq2 }
        };
        int id = 0;
        var equipments = _fixture.Build<Equipment>().With(e => e.Id, () => ++id).Without(e => e.Rooms).CreateMany(6).ToList();
        var rooms = new List<Room>
            {
                new Room { Id = 1, Available = true, Equipments = new List<Equipment> { equipments[0], equipments[1], equipments[2] }},
                new Room { Id = 2, Available = true, Equipments = new List<Equipment> { equipments[0], equipments[3], equipments[4] }},
                new Room { Id = 3, Available = true, Equipments = new List<Equipment> { equipments[1], equipments[2], equipments[4] }},
                new Room { Id = 4, Available = true, Equipments = new List<Equipment> { equipments[2], equipments[3], equipments[4] }}
            };

        _roomRepository.GetBigEnoughRoomsAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(rooms);
        _mapper.Map<List<RoomListDTO>>(Arg.Any<List<Room>>()).Returns(rl => ((List<Room>)rl[0]).Select( r => new RoomListDTO()
        {
            Id = r.Id,
        }).ToList());

        // Act
        var result = await _roomService.GetAvailableRoomsAsync(query);

        //Assert
        result.ToList().Count.Should().Be(expectedResult);
    }
}

public class EquipmentTestData : TheoryData<int, int, int>
{
    public EquipmentTestData()
    {
        Add(1,2,1);
        Add(4,5,2);
        Add(1,6,0);
    }
}
