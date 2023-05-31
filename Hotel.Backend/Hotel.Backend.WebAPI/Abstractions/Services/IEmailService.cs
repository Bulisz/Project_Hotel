using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IEmailService
    {
        EmailDTO CreatingVerificationEmail(string? email, string lastName, string firstName, string url);
        Task SendEmailAsync(EmailDTO message);
    }
}