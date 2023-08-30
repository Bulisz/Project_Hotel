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
    public class CalendarServiceTests
    {
        private CalendarService _calendarService;
        private Mock<IReservationRepository> _reservationRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
           
            _calendarService = new CalendarService(
                _reservationRepositoryMock.Object
            );
        }

        [TestMethod]
        public async Task GetAllDaysOfMonthAsync_Test()
        { 
            //Arrange
            DateTime testDay = new DateTime(2023, 1, 1);
            int year = testDay.Year;
            int month = testDay.Month;

            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    ApplicationUser = new ApplicationUser { Id = "UserId1" },
                    BookingFrom = testDay.AddDays(2),
                    BookingTo = testDay.AddDays(5),
                    Room = new Room { Id = 1 }
                },
                new Reservation
                {
                    Id = 2,
                    ApplicationUser = new ApplicationUser { Id = "UserId2" },
                    BookingFrom = testDay.AddDays(2),
                    BookingTo = testDay.AddDays(5),
                    Room = new Room { Id = 2 }
                }
            };

            

            _reservationRepositoryMock.Setup(m => m.GetAllReservationsAsync())
            .ReturnsAsync(reservations);

            // Act
            var result = await _calendarService.GetAllDaysOfMonthAsync(year, month);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(31, result.Count);
            Assert.AreEqual(1, result[2].RoomStatus[0].RoomNumber);
            Assert.AreEqual(2, result[2].RoomStatus[1].RoomNumber);
            Assert.AreEqual("first", result[2].RoomStatus[1].ReservationStatus);
            Assert.AreEqual("last", result[4].RoomStatus[1].ReservationStatus);


        }

    }
}
