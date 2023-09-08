using Hotel.Backend.WebAPI.Database;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.UnitTests.Repositories;

[TestClass]
public class RoomRepositoryTests
{
    private HotelDbContext _dbContext = null!;
    private RoomRepository _roomRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlite("Data Source = :memory:");
        _dbContext = new HotelDbContext(optionsBuilder.Options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _roomRepository = new RoomRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task GetAllRoomTest()
    {
        //Arrange
        var roomList = new List<Room>()
        {
            new()
            {
                Available = true,
                Name = "Test1",
            },
            new()
            {
                Available = false,
                Name = "Test2",
            },
            new()
            {
                Available = true,
                Name = "Test3",
            },
            new()
            {
                Available = false,
                Name = "Test4",
            },
        };
        _dbContext.Rooms.AddRange(roomList);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _roomRepository.GetAllRoomsAsync();

        //Assert
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public async Task GetRoomByIdTest()
    {
        //Arrange
        var roomList = new List<Room>()
        {
            new()
            {
                Available = true,
                Name = "Test1",
            },
            new()
            {
                Available = false,
                Name = "Test2",
            },
            new()
            {
                Available = true,
                Name = "Test3",
            },
            new()
            {
                Available = false,
                Name = "Test4",
            },
        };
        _dbContext.Rooms.AddRange(roomList);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _roomRepository.GetRoomByIdAsync(2);

        //Assert
        Assert.AreEqual("Test2", result!.Name);
    }

    [TestMethod]
    public async Task GetRoomsByNameTest()
    {
        //Arrange
        var roomList = new List<Room>()
        {
            new()
            {
                Available = true,
                Name = "Test1",
            },
            new()
            {
                Available = false,
                Name = "Test2",
            },
            new()
            {
                Available = true,
                Name = "Test3",
            },
            new()
            {
                Available = false,
                Name = "Test4",
            },
        };
        _dbContext.Rooms.AddRange(roomList);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _roomRepository.GetRoomsByNamesAsync(new() { "Test2", "Test4"});

        //Assert
        Assert.AreEqual(2, result!.Count);
    }

    [TestMethod]

    public async Task GetBigEnoughRoomsTest()
    {
        //Arrange
        var res1 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-01"), BookingTo = DateTime.Parse("2000-01-05") };
        var res2 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-05"), BookingTo = DateTime.Parse("2000-01-10") };
        var res3 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-07"), BookingTo = DateTime.Parse("2000-01-25") };

        var res4 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-01"), BookingTo = DateTime.Parse("2000-01-05") };
        var res5 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-05"), BookingTo = DateTime.Parse("2000-01-10") };
        var res6 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-20"), BookingTo = DateTime.Parse("2000-01-30") };

        var res7 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-01"), BookingTo = DateTime.Parse("2000-01-05") };
        var res8 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-07"), BookingTo = DateTime.Parse("2000-01-25") };
        var res9 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-20"), BookingTo = DateTime.Parse("2000-01-30") };

        var res10 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-05"), BookingTo = DateTime.Parse("2000-01-10") };
        var res11 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-07"), BookingTo = DateTime.Parse("2000-01-25") };
        var res12 = new Reservation() { BookingFrom = DateTime.Parse("2000-01-20"), BookingTo = DateTime.Parse("2000-01-30") };

        var room1 = new Room() {
            Available = true,
            Reservations = new List<Reservation> { res1, res2, res3 },
            NumberOfBeds = 1,
            MaxNumberOfDogs = 4,
        };
        var room2 = new Room()
        {
            Available = true,
            Reservations = new List<Reservation> { res4, res5, res6 },
            NumberOfBeds = 2,
            MaxNumberOfDogs = 3,
        };
        var room3 = new Room()
        {
            Available = true,
            Reservations = new List<Reservation> { res7, res8, res9 },
            NumberOfBeds = 3,
            MaxNumberOfDogs = 2,
        };
        var room4 = new Room()
        {
            Available = true,
            Reservations = new List<Reservation> { res10, res11, res12 },
            NumberOfBeds = 4,
            MaxNumberOfDogs = 1,
        };
        var room5 = new Room()
        {
            Available = false,
            NumberOfBeds = 4,
            MaxNumberOfDogs = 4,
        };
        _dbContext.Rooms.Add(room1);
        _dbContext.Rooms.Add(room2);
        _dbContext.Rooms.Add(room3);
        _dbContext.Rooms.Add(room4);
        _dbContext.Rooms.Add(room5);
        await _dbContext.SaveChangesAsync();

        //Act
        var result1 = await _roomRepository.GetBigEnoughRoomsAsync(1, 1, DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-10"));
        var result2 = await _roomRepository.GetBigEnoughRoomsAsync(3, 1, DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-10"));
        var result3 = await _roomRepository.GetBigEnoughRoomsAsync(5, 1, DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-10"));

        var result4 = await _roomRepository.GetBigEnoughRoomsAsync(1, 1, DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-10"));
        var result5 = await _roomRepository.GetBigEnoughRoomsAsync(1, 3, DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-10"));
        var result6 = await _roomRepository.GetBigEnoughRoomsAsync(1, 5, DateTime.Parse("2000-02-01"), DateTime.Parse("2000-02-10"));

        var result7 = await _roomRepository.GetBigEnoughRoomsAsync(1, 1, DateTime.Parse("2000-01-05"), DateTime.Parse("2000-01-07"));
        var result8 = await _roomRepository.GetBigEnoughRoomsAsync(1, 1, DateTime.Parse("2000-01-08"), DateTime.Parse("2000-01-09"));
        var result9 = await _roomRepository.GetBigEnoughRoomsAsync(1, 1, DateTime.Parse("2000-01-30"), DateTime.Parse("2000-01-31"));

        var result10 = await _roomRepository.GetBigEnoughRoomsAsync(1, 3, DateTime.Parse("2000-01-05"), DateTime.Parse("2000-01-07"));
        var result11 = await _roomRepository.GetBigEnoughRoomsAsync(4, 1, DateTime.Parse("2000-01-05"), DateTime.Parse("2000-01-07"));
        var result12 = await _roomRepository.GetBigEnoughRoomsAsync(2, 2, DateTime.Parse("2000-01-30"), DateTime.Parse("2000-01-31"));

        //Arrange
        Assert.AreEqual(4, result1.Count);
        Assert.AreEqual(2, result2.Count);
        Assert.AreEqual(0, result3.Count);

        Assert.AreEqual(4, result4.Count);
        Assert.AreEqual(2, result5.Count);
        Assert.AreEqual(0, result6.Count);

        Assert.AreEqual(1, result7.Count);
        Assert.AreEqual(0, result8.Count);
        Assert.AreEqual(4, result9.Count);

        Assert.AreEqual(0, result10.Count);
        Assert.AreEqual(0, result11.Count);
        Assert.AreEqual(2, result12.Count);
    }

    [TestMethod]
    public async Task CreateRoomTest()
    {
        //Arrange
        var room = new Room()
        {
            Available = true,
            Name = "A",
        };

        //Act
        var result = await _roomRepository.CreateRoomAsync(room);

        //Assert
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public async Task DeleteRoomTest()
    {
        //Arrange
        var room = new Room()
        {
            Available = true,
            Name = "A",
        };
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();

        //Act
        await _roomRepository.DeleteRoomAsync(1);

        //Assert
        Assert.IsFalse(room.Available);
    }

    [TestMethod]
    public async Task ModifyRoomTest()
    {
        //Arrange
        var room = new Room()
        {
            Available = true,
            Name = "A",
        };
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();
        var roomToModify = await _dbContext.Rooms.FindAsync(1);
        roomToModify.Name = "B";

        //Act
        await _roomRepository.ModifyRoomAsync(roomToModify);

        //Assert
        Assert.AreEqual("B", room.Name);
    }

    [TestMethod]
    public async Task DeleteImageOfRoom()
    {
        //Arrange
        var image = new Image() { ImageUrl = "url" };
        var room = new Room()
        {
            Available = true,
            Name = "A",
            Images = new List<Image>() { image }
        };
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();

        //Act
        await _roomRepository.DeleteImageOfRoomAsync("url");
        var result = await _dbContext.Rooms.FindAsync(1);

        //Assert
        Assert.AreEqual(0, result!.Images.Count);
    }
}
