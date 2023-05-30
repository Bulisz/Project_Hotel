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
            throw new Exception("Küldés sikertelen");
        }
        finally
        {
            await smtp.DisconnectAsync(true);
            smtp.Dispose();
        }


    }
}
