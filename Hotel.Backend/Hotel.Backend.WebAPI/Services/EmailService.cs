using Hotel.Backend.WebAPI.Models.DTO;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Hotel.Backend.WebAPI.Abstractions.Services;

namespace Hotel.Backend.WebAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }
    public async Task SendEmailAsync(EmailDTO message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("pearlie44@ethereal.email"));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = message.Body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config.GetValue<string>("EmailConfig:Host"), 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config.GetValue<string>("EmailConfig:Username"), _config.GetValue<string>("EmailConfig:Password"));
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
