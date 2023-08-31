using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;
using Hotel.Backend.WebAPI.Models.DTO.StatisticsDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HotelBackend.UnitTests.Controllers
{
    [TestClass]
    public class StatisticsControllerTests
    {
        private StatisticsController _statisticsController;
        private Mock<IStatisticsService> _statisticsServiceMock;
        private Mock<ILogger<StatisticsController>> _loggerMock;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<StatisticsController>>();
            _statisticsServiceMock = new Mock<IStatisticsService>();

            _statisticsController = new StatisticsController
                (
                    _statisticsServiceMock.Object,
                    _loggerMock.Object
                );
        }

        [TestMethod]
        public async Task GetRoomMonthStatTests_Valid()
        {
            //Arrange
            var expectedResult = new List<RoomReservationPerMonthDTO>
            {
                new RoomReservationPerMonthDTO
                {
                    Id = 1,
                    Name = "Bodri",
                    Percentage = 50
                },
                new RoomReservationPerMonthDTO
                {
                    Id = 2,
                    Name = "Buksi",
                    Percentage = 10
                }
            };

            _statisticsServiceMock.Setup(m => m.GetRoomMonthStatAsync(2023, 1)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _statisticsController.GetRoomMonthStat(2023, 1);
            var okResult = (OkObjectResult)actualResult.Result!;
            var result = (IEnumerable<RoomReservationPerMonthDTO>)okResult.Value!;

            // Assert
            Assert.IsInstanceOfType(actualResult.Result, typeof(OkObjectResult));
            CollectionAssert.AreEqual(expectedResult, result.ToList());
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task GetRoomMonthStatTests_Error()
        {
            // Arrange
            var error = new ArgumentException("Hiba történt");
            _statisticsServiceMock.Setup(m => m.GetRoomMonthStatAsync(2023, 1)).Throws(error);

            // Act
            var result = await _statisticsController.GetRoomMonthStat(2023, 1);
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Hiba történt", objectResult.Value);
        }


        [TestMethod]
        public async Task GetYearStat_Valid()
        {
            //Arrange
            var inputQuery = new YearStatQueryDTO
            {
                Year = 2023,
                ChoosedRooms = new List<string> { "Bodri", "Buksi" }
            };

            var expectedResult = new List<StatisticsPerYearDTO> 
            {
                new StatisticsPerYearDTO 
                {
                    Name = "Bodri",
                    Data = new List<double> {10, 20}
                },
                new StatisticsPerYearDTO
                {
                    Name = "Buksi",
                    Data = new List<double> {10, 50}
                }
            };

            _statisticsServiceMock.Setup(m => m.GetYearStatAsync(inputQuery)).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await _statisticsController.GetYearStat(inputQuery);
            var okResult = (OkObjectResult)actualResult.Result!;
            var result = (IEnumerable<StatisticsPerYearDTO>)okResult.Value!;

            // Assert
            Assert.IsInstanceOfType(actualResult.Result, typeof(OkObjectResult));
            CollectionAssert.AreEqual(expectedResult, result.ToList());
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task GetYearStat_Error()
        {
            // Arrange
            var inputQuery = new YearStatQueryDTO
            {
                Year = 2023,
                ChoosedRooms = new List<string> { "Bodri", "Buksi" }
            };

            var error = new ArgumentException("Hiba történt");

            _statisticsServiceMock.Setup(m => m.GetYearStatAsync(inputQuery)).Throws(error);

            // Act
            var result = await _statisticsController.GetYearStat(inputQuery);
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Hiba történt", objectResult.Value);
        }
    }
}
