using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Helpers;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Models.DTO.CalendarDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class ReservationsControllerTests
{

    private ReservationsController _controller;
    private Mock<IReservationService> _reservationServiceMock;
    private Mock<ILogger<ReservationsController>> _loggerMock;
    private Mock<ICalendarService> _calendarServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ReservationsController>>();
        _calendarServiceMock = new Mock<ICalendarService>();
        _reservationServiceMock = new Mock<IReservationService>();
        _controller = new ReservationsController(_reservationServiceMock.Object, _calendarServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAllReservationsTests()
    {
        // Arrange
        var startingDate = DateTime.Today.AddDays(2);
        var leavingDate = DateTime.Today.AddDays(5);
        var expectedReservationsListItems = new List<ReservationListItemDTO>
        {
            new ReservationListItemDTO {
                Id = 1,
                RoomName = "Bodri",
                UserId = "UserId",
                BookingFrom = startingDate,
                BookingTo = leavingDate
            },
            new ReservationListItemDTO {
                Id = 2,
                RoomName = "Buksi",
                UserId = "UserId2",
                BookingFrom = startingDate,
                BookingTo = leavingDate
            }
        };

        _reservationServiceMock.Setup(m => m.GetAllReservationsAsync()).ReturnsAsync(expectedReservationsListItems);

        // Act
        var actualReservationDetailsListItemDTO = await _controller.GetAllReservations();

        // Assert
        Assert.IsInstanceOfType(actualReservationDetailsListItemDTO.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)actualReservationDetailsListItemDTO.Result!;
        var result = (IEnumerable<ReservationListItemDTO>)okResult.Value!;

        CollectionAssert.AreEqual(expectedReservationsListItems, result.ToList());

        Assert.AreEqual(expectedReservationsListItems, result);
    }

    [TestMethod]
    public async Task CreateReservationForRoom_ValidRequest_ReturnOk()
    {
        // Arrange
        var startingDate = DateTime.Today.AddDays(2);
        var leavingDate = DateTime.Today.AddDays(5);
        var request = new ReservationRequestDTO
        {
            UserId = "UserId", 
            RoomId = 3, 
            BookingFrom = startingDate, 
            BookingTo= leavingDate
        };

        var response = new ReservationDetailsDTO
        {
            Id = 1,
            UserId = "UserId",
            RoomId = 3,
            BookingFrom = startingDate,
            BookingTo = leavingDate
        };

        _reservationServiceMock.Setup(m => m.CreateReservationAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateReservationForRoom(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result.Result;
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task CreateReservationForRoom_HotelException_ReturnsStatusCode()
    {
        // Arrange
        var startingDate = DateTime.Today.AddDays(5);
        var leavingDate = DateTime.Today.AddDays(2);
        var request = new ReservationRequestDTO
        {
            UserId = "UserId",
            RoomId = 3,
            BookingFrom = startingDate,
            BookingTo = leavingDate
        };

        var hotelErrors = new List<HotelFieldError>
        {
            new HotelFieldError("BookingTo", "A távozásnak később kell lennie, mint az érkezésnek"),
            new HotelFieldError("BookingFrom", "A távozásnak később kell lennie, mint az érkezésnek")
        };

        var hotelException = new HotelException(HttpStatusCode.BadRequest, hotelErrors, "One or more hotel errors occurred.");

        _reservationServiceMock.Setup(m => m.CreateReservationAsync(request)).Throws(hotelException);

        // Act
        var result = await _controller.CreateReservationForRoom(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
        var objectResult = (ObjectResult)result.Result;
        Assert.AreEqual((int)hotelException.Status, objectResult.StatusCode);

        var errorResponse = objectResult.Value;

        var errors = errorResponse.GetType().GetProperty("errors")?.GetValue(errorResponse) as List<HotelFieldError>;
        Assert.IsNotNull(errors);
        Assert.AreEqual(hotelErrors.Count, errors.Count);

        for (int i = 0; i < hotelErrors.Count; i++)
        {
            var expectedError = hotelErrors[i];
            var actualError = errors[i];
            Assert.AreEqual(expectedError.FieldName, actualError.FieldName);
            Assert.AreEqual(expectedError.FieldErrorMessage, actualError.FieldErrorMessage);
        }
    }

    [TestMethod]
    public async Task GetMyOwnReservations_ReturnsOkResultWithReservations()
    {
        // Arrange
        string userId = "user123";
        var reservations = new List<ReservationListItemDTO>
            {
                new ReservationListItemDTO { Id = 1, RoomName = "Room 1", UserId = "user123" },
                new ReservationListItemDTO { Id = 2, RoomName = "Room 2", UserId = "user123" }
            };

        _reservationServiceMock.Setup(service => service.GetMyOwnReservationsAsync(userId))
                               .ReturnsAsync(reservations);

        // Act
        ActionResult<IEnumerable<ReservationListItemDTO>> actionResult = await _controller.GetMyOwnReservations(userId);

        // Assert
        Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));

        var okResult = actionResult.Result as OkObjectResult;
        var result = okResult.Value;
        Assert.AreEqual(reservations, result);
    }

    [TestMethod]
    public async Task DeleteReservationForRoom_ReturnNoContent()
    {
        // Arrange
        _reservationServiceMock.Setup(m => m.DeleteReservationAsync(1));

        // Act
        var result = await _controller.DeleteReservation(1);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task DeleteReservationForRoom_WithNoReservation()
    {
        // Arrange
        var error = new ArgumentException("Nincs ilyen foglalás");
        _reservationServiceMock.Setup(m => m.DeleteReservationAsync(1)).Throws(error);

        // Act
        var result = await _controller.DeleteReservation(1);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        var objectResult = (ObjectResult)result;
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Nincs ilyen foglalás", objectResult.Value);
    }

    [TestMethod]
    public async Task GetThisMonthCalendarTests_ValidInput()
    {
        // Arrange
        List<ThisMonthCalendarDTO> expectedResult = new List<ThisMonthCalendarDTO>
        {
            new ThisMonthCalendarDTO
            {
                DateNumber = 1,
                WeekDayNumber = 2,
                RoomStatus = new List<DailyReservationDTO>
                {
                    new DailyReservationDTO 
                    {
                        RoomNumber = 1,
                    }
                }
            },
            new ThisMonthCalendarDTO
            {
                DateNumber = 2,
                WeekDayNumber = 3,
                RoomStatus = new List<DailyReservationDTO>
                {
                    new DailyReservationDTO
                    {
                        RoomNumber = 2,
                    }
                }
            }
        };

        _calendarServiceMock.Setup(m => m.GetAllDaysOfMonthAsync(2023, 1)).ReturnsAsync(expectedResult);

        // Act
        var actualThisMonthCalendarDTO = await _controller.GetThisMonthCalendar(2023, 1);

        // Assert
        Assert.IsInstanceOfType(actualThisMonthCalendarDTO.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)actualThisMonthCalendarDTO.Result!;
        var result = (IEnumerable<ThisMonthCalendarDTO>)okResult.Value!;

        CollectionAssert.AreEqual(expectedResult, result.ToList());

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task GetThisMonthCalendarTests_Error()
    {
        // Arrange
        var error = new ArgumentException("Hiba történt");
        _calendarServiceMock.Setup(m => m.GetAllDaysOfMonthAsync(2023, 1)).Throws(error);

        // Act
        var result = await _controller.GetThisMonthCalendar(2023, 1);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));

        var objectResult = (ObjectResult)result.Result;
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Hiba történt", objectResult.Value);
    }
}