using Hotel.Backend.WebAPI.Abstractions;
using Hotel.Backend.WebAPI.Controllers;
using Hotel.Backend.WebAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelBackend.UnitTests.Controllers;

[TestClass]
public class ReservationsControllerTests
{
    [TestMethod]
    public async Task GetAllReservationsTests()
    {
        // Arrange
        var startingDate = DateTime.Today.AddDays(2);
        var leavingDate = DateTime.Today.AddDays(5);
        var expectedReservationsListItems = new List<ReservationDetailsListItemDTO>
        {
            new ReservationDetailsListItemDTO {Id = 1, RoomName = "Bodri", UserId = "UserId", BookingFrom = startingDate, BookingTo = leavingDate},
            new ReservationDetailsListItemDTO {Id = 2, RoomName = "Buksi", UserId = "UserId2", BookingFrom = startingDate, BookingTo = leavingDate}
        };

        var reservationServiceMock = new Mock<IReservationService>();
        reservationServiceMock.Setup(m => m.GetAllReservationsAsync()).ReturnsAsync(expectedReservationsListItems);
        var reservationsController = new ReservationsController(reservationServiceMock.Object);

        // Act
        var actualReservationDetailsListItemDTO = await reservationsController.GetAllReservations();

        // Assert
        Assert.IsInstanceOfType(actualReservationDetailsListItemDTO.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)actualReservationDetailsListItemDTO.Result!;
        var result = (IEnumerable<ReservationDetailsListItemDTO>)okResult.Value!;

        CollectionAssert.AreEqual(expectedReservationsListItems, result.ToList());

        Assert.AreEqual(expectedReservationsListItems, result);

    }

}