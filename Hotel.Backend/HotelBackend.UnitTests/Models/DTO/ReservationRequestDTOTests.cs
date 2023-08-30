using Hotel.Backend.WebAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace HotelBackend.UnitTests.Models.DTO;

[TestClass]
public class ReservationRequestDTOTests
{
    [TestMethod]
    public void ValideBookingDates()
    {
        // Arrange
        var bookingFrom = DateTime.Today.AddDays(1);
        var bookingTo = DateTime.Today.AddDays(3);
        var reservationRequest = new ReservationRequestDTO
        {
            UserId = "User123",
            RoomId = 3,
            BookingFrom = bookingFrom,
            BookingTo = bookingTo,
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(reservationRequest);

        // Act
        bool isValid = Validator.TryValidateObject(reservationRequest, validationContext, validationResults, validateAllProperties: true);

        // Assert
        Assert.IsTrue(isValid);
    }


    [DataTestMethod]
    [DataRow("2023-05-23")]
    [DataRow("2022-08-15")]
    [DataRow("2021-03-01")]
    public void BookingFromIsInThePast(string bookingFrom)
    {

        // Arrange
        var dateTime = DateTime.Parse(bookingFrom);
        var inTheFuture = DateTime.Today.AddDays(1);
        var reservationRequest = new ReservationRequestDTO
        {
            UserId = "UserId123",
            RoomId = 1,
            BookingFrom = dateTime,
            BookingTo = inTheFuture,
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(reservationRequest);

        // Act
        bool isValid = Validator.TryValidateObject(reservationRequest, validationContext, validationResults, validateAllProperties: true);

        // Assert
        Assert.IsFalse(isValid);
        Assert.AreEqual(1, validationResults.Count);
        Assert.AreEqual("Csak a jövőbeni dátumokra lehetséges foglalni", validationResults[0].ErrorMessage);
        Assert.AreEqual(1, validationResults[0].MemberNames.Count());
    }

    [DataTestMethod]
    [DataRow("2023-05-23")]
    [DataRow("2022-08-15")]
    [DataRow("2021-03-01")]
    public void BookingToIsInThePast(string bookingTo)
    {

        // Arrange
        var dateTime = DateTime.Parse(bookingTo);
        var inTheFuture = DateTime.Today.AddDays(1);
        var reservationRequest = new ReservationRequestDTO
        {
            UserId = "UserId123",
            RoomId = 1,
            BookingFrom = inTheFuture,
            BookingTo = dateTime,
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(reservationRequest);

        // Act
        bool isValid = Validator.TryValidateObject(reservationRequest, validationContext, validationResults, validateAllProperties: true);

        // Assert
        Assert.IsFalse(isValid);
        Assert.AreEqual(1, validationResults.Count);
        Assert.AreEqual("Csak a jövőbeni dátumokra lehetséges foglalni", validationResults[0].ErrorMessage);
        Assert.AreEqual("BookingTo", validationResults[0].MemberNames.First());
    }
}
