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
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config,ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public EmailDTO CreatingVerificationEmail(string emailAddress, string lastName, string firstName, string url)
    {
        EmailDTO email = new EmailDTO
        {
            To = emailAddress,
            Subject = "Regisztráció megerősítése",
            Body = $"Kedves {lastName} {firstName}! <br>" +
            $" Erre a linkre kattintva megerősítheted honlapunkon a regisztrációt: <a href=\"{url}\"> aktiváló link</a>. <br>" +
            "A link 15 percig lesz érvényes."
        };

        return email;
    }

    public async Task SendEmailAsync(EmailDTO message)
    {
        MimeMessage email = CreateEmailMessage(message);
        await SendAsync(email);
    }

    private MimeMessage CreateEmailMessage(EmailDTO message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("DogHotel", _config.GetValue<string>("EmailConfig:Username")));
        email.To.Add(new MailboxAddress("email", message.To));
        email.Subject = message.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = message.Body };

        return email;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(_config.GetValue<string>("EmailConfig:Host"), 465, true);
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            await smtp.AuthenticateAsync(_config.GetValue<string>("EmailConfig:Username"), _config.GetValue<string>("EmailConfig:Password"));

            await smtp.SendAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "nincs email");
            throw new Exception("Küldés sikertelen",ex);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
            smtp.Dispose();
        }


    }
}
