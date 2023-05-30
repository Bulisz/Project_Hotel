using Hotel.Backend.WebAPI.Models.DTO;

namespace Hotel.Backend.WebAPI.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO message);
    }
}