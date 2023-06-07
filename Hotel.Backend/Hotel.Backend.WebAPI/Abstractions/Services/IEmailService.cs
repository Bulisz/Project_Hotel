using Hotel.Backend.WebAPI.Models;
using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IEmailService
    {
        EmailDTO CreatingVerificationEmail(string? email, string lastName, string firstName, string url);
        Task SendEmailAsync(EmailDTO message);
        EmailDTO CreatingForgottenPasswordEmail(string emailAddress, string username, string url);
        EmailDTO CreatingReservationConfirmationEmail(ApplicationUser user, Reservation reservation);
        EmailDTO CreatingNotificationOfReservation(ApplicationUser user, Reservation reservation);
        EmailDTO CreatingCancelReservationEmail(Reservation reservation);
        EmailDTO CreatingNotificationOfCancelation(Reservation reservation);
    }

}