using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using Moq;

namespace HotelBackend.UnitTests.Services;

[TestClass]
public class ReservationServiceTests
{
    private ReservationService _reservationService;
    private Mock<IReservationRepository> _reservationRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IRoomRepository> _roomRepositoryMock;

    [TestInitialize]
    public void Setup()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();

        _reservationService = new ReservationService(
            _reservationRepositoryMock.Object,
            _userRepositoryMock.Object,
            _roomRepositoryMock.Object
        );
    }

    [TestMethod]
    public async Task CreateReservationAsync_ValidRequest()
    {
        // Arrange
        var request = new ReservationRequestDTO
        {
            UserId = "UserId",
            RoomId = 3,
            BookingFrom = DateTime.Today.AddDays(2),
            BookingTo = DateTime.Today.AddDays(5)
        };

        var user = new ApplicationUser { Id = request.UserId };
        var room = new Room { Id = request.RoomId };
        var reservation = new Reservation
        {
            Id = 1,
            ApplicationUser = user,
            BookingFrom = request.BookingFrom,
            BookingTo = request.BookingTo,
            Room = room
        };

        _userRepositoryMock.Setup(m => m.GetUserByIdAsync(request.UserId))
            .ReturnsAsync(new UserDetailsDTO { User = user });
        _roomRepositoryMock.Setup(m => m.GetRoomByIdAsync(request.RoomId))
            .ReturnsAsync(room);
        _reservationRepositoryMock.Setup(m => m.CreateReservationAsync(It.IsAny<Reservation>()))
            .ReturnsAsync(reservation);

        // Act
        var result = await _reservationService.CreateReservationAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(reservation.Id, result.Id);
        Assert.AreEqual(room.Id, result.RoomId);
        Assert.AreEqual(user.Id, result.UserId);
        Assert.AreEqual(request.BookingFrom, result.BookingFrom);
        Assert.AreEqual(request.BookingTo, result.BookingTo);
    }

    [TestMethod]
    public async Task CreateReservationAsync_RoomNotAvailable_ThrowsHotelException()
    {
        // Arrange
        var request = new ReservationRequestDTO
        {
            UserId = "UserId",
            RoomId = 3,
            BookingFrom = DateTime.Today.AddDays(2),
            BookingTo = DateTime.Today.AddDays(5)
        };

        var user = new ApplicationUser { Id = request.UserId };
        var room = new Room { Id = request.RoomId };
        var existingReservation = new Reservation
        {
            ApplicationUser = user,
            BookingFrom = DateTime.Today.AddDays(3),
            BookingTo = DateTime.Today.AddDays(6),
            Room = room
        };
        room.Reservations.Add(existingReservation);

        _userRepositoryMock.Setup(m => m.GetUserByIdAsync(request.UserId))
            .ReturnsAsync(new UserDetailsDTO { User = user });
        _roomRepositoryMock.Setup(m => m.GetRoomByIdAsync(request.RoomId))
            .ReturnsAsync(room);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<HotelException>(
            async () => await _reservationService.CreateReservationAsync(request)
        );
    }

    [TestMethod]
    public async Task CreateReservationAsync_InvalidRequest_ThrowsHotelException()
    {
        // Arrange
        var request = new ReservationRequestDTO
        {
            UserId = "UserId",
            RoomId = 3,
            BookingFrom = DateTime.Today.AddDays(5),
            BookingTo = DateTime.Today.AddDays(2)
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<HotelException>(
            async () => await _reservationService.CreateReservationAsync(request)
        );
    }

    [TestMethod]
    public async Task GetAllReservationsAsync_ReturnsListOfReservationDetailsListItemDTO()
    {
        // Arrange
        var reservations = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    BookingFrom = DateTime.Today.AddDays(2),
                    BookingTo = DateTime.Today.AddDays(5),
                    ApplicationUser = new ApplicationUser { Id = "UserId" },
                    Room = new Room { Name = "Bodri" }
                },
                new Reservation
                {
                    Id = 2,
                    BookingFrom = DateTime.Today.AddDays(2),
                    BookingTo = DateTime.Today.AddDays(5),
                    ApplicationUser = new ApplicationUser { Id = "UserId2" },
                    Room = new Room { Name = "Buksi" }
                }
            };

        _reservationRepositoryMock.Setup(m => m.GetAllReservationsAsync())
            .ReturnsAsync(reservations);

        // Act
        var result = await _reservationService.GetAllReservationsAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(reservations.Count, result.Count);

        for (int i = 0; i < reservations.Count; i++)
        {
            var expectedReservation = reservations[i];
            var actualReservation = result[i];

            Assert.AreEqual(expectedReservation.Id, actualReservation.Id);
            Assert.AreEqual(expectedReservation.BookingFrom, actualReservation.BookingFrom);
            Assert.AreEqual(expectedReservation.BookingTo, actualReservation.BookingTo);
            Assert.AreEqual(expectedReservation.ApplicationUser.Id, actualReservation.UserId);
            Assert.AreEqual(expectedReservation.Room.Name, actualReservation.RoomName);
        }
    }
}

