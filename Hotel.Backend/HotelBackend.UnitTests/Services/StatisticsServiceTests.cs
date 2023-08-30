using Hotel.Backend.WebAPI.Abstractions.Repositories;
using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;
using Hotel.Backend.WebAPI.Services;
using Moq;

namespace HotelBackend.UnitTests.Services
{
    [TestClass]
    public class StatisticsServiceTests
    {
        private StatisticsService _statisticsService;
        private Mock<IRoomRepository> _roomRepositoryMock;
        

        [TestInitialize]
        public void Setup()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();

            _statisticsService = new StatisticsService(
                _roomRepositoryMock.Object
            );
        }

        [TestMethod]
        public async Task GetRoomMonthStatAsync_Test()
        {
            //Arrange
            var allRooms = new List<Room>
            {
                new Room
                {
                    Id = 1,
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            BookingFrom = new DateTime (2023, 4, 1),
                            BookingTo = new DateTime (2023, 4, 16)
                        },
                        new Reservation
                        {
                            Id = 2,
                            BookingFrom = new DateTime (2023, 6, 1),
                            BookingTo = new DateTime (2023, 6, 4)
                        }
                    }
                },
                new Room 
                {
                    Id = 2,
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 3,
                            BookingFrom = new DateTime (2023, 6, 1),
                            BookingTo = new DateTime (2023, 6, 16)
                        },
                        new Reservation
                        {
                            Id = 4,
                            BookingFrom = new DateTime (2023, 4, 1),
                            BookingTo = new DateTime (2023, 4, 4)
                        }
                    }
                }
            };

            _roomRepositoryMock.Setup(m => m.GetAllRoomsAsync())
           .ReturnsAsync(allRooms);

            // Act
            var result = (await _statisticsService.GetRoomMonthStatAsync(2023, 4)).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(50, result[0].Percentage);
            Assert.AreEqual(10, result[1].Percentage);
        }
    }
}
