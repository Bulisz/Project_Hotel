using Hotel.Backend.WebAPI.Models.DTO;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Hotel.Backend.WebAPI.Abstractions.Services;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Hotel.Backend.WebAPI.Models;

namespace Hotel.Backend.WebAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public EmailDTO CreatingVerificationEmail(string? emailAddress, string lastName, string firstName, string url)
    {
        EmailDTO email = new()
        {
            To = emailAddress!,
            Subject = "Regisztráció megerősítése",
            Body = $"Kedves {lastName} {firstName}! <br>" +
            $" Erre a linkre kattintva megerősítheted honlapunkon a regisztrációt: <a href=\"{url}\"> aktiváló link</a>. <br>" +
            "A link 15 percig lesz érvényes."
        };

        return email;
    }

    public EmailDTO CreatingForgottenPasswordEmail(string emailAddress, string username, string url)
    {
        EmailDTO email = new()
        {
            To = emailAddress,
            Subject = "Elfelejtett jelszó pótlása",
            Body = $"Kedves {username}! <br>" +
            $" Erre a linkre kattintva új jelszót adhatsz meg a regisztrált fiókodhoz: <a href=\"{url}\"> link</a>. <br>" +
            "A link 15 percig lesz érvényes."
        };

        return email;
    }

    public EmailDTO CreatingReservationConfirmationEmail(ApplicationUser user, Reservation reservation)
    {
        EmailDTO email = new()
        {
            To = user.Email!,
            Subject = "Sikeres foglalás",
            Body = $"Kedves {user.LastName} {user.FirstName}! <br>" +
            $"A foglalásodhoz tartozó adatok: <br>" +
            $"Időpont: {reservation.BookingFrom.ToString("yyyy. MM. dd.")} – {reservation.BookingTo.ToString("yyyy. MM. dd.")} <br>" +
            $"Szoba: {reservation.Room.Name} <br>" +
            $"A szobák az érkezés napján 14 órától átvehetők és a távozás napján 11 óráig kell elhagynotok a szállást. <br>" +
            $"Köszönjük, hogy minket választottatok, már nagyon várunk Titeket! <br>" +
            $"A DogHotel csapata",
        };

        return email;
    }

    public EmailDTO CreatingCancelReservationEmail(Reservation reservation)
    {
        EmailDTO email = new EmailDTO
        {
            To = reservation.ApplicationUser.Email!,
            Subject = "Foglalás törlése",
            Body = $"Kedves {reservation.ApplicationUser.LastName} {reservation.ApplicationUser.FirstName}! <br>" +
            $"Sajnálattal fogadtuk, hogy lemondtad a {reservation.BookingFrom.ToString("yyyy. MM. dd.")}-tól " +
            $" {reservation.BookingTo.ToString("yyyy. MM. dd.")}-ig szóló foglalásodat. <br>" +
            $"Reméljük, még találkozunk a jövőben! <br>" +
            $"A DogHotel csapata"
        };

        return email;
    }

    public EmailDTO CreatingNotificationOfReservation(ApplicationUser user, Reservation reservation)
    {
        EmailDTO notification = new EmailDTO
        {
            To = _config.GetValue<string>("EmailConfig:Username")!,
            Subject = "ÉRTESÍTÉS: új foglalás",
            Body = "Új foglalás történt <br>" +
            "Adatok: <br>" +
            $"Időpont: {reservation.BookingFrom.ToString("yyyy. MM. dd.")} – {reservation.BookingTo.ToString("yyyy. MM. dd.")} <br>" +
            $"Szoba: {reservation.Room.Name} <br>" +
            $"Vendég: {user.LastName} {user.FirstName}, email: {user.Email}"
        };

        return notification;
    }

    public EmailDTO CreatingNotificationOfCancelation(Reservation reservation)
    {
        EmailDTO notification = new EmailDTO
        {
            To = _config.GetValue<string>("EmailConfig:Username")!,
            Subject = "ÉRTESÍTÉS: foglalás törlése",
            Body = "Foglalás törlésre került <br>" +
            "Adatok: <br>" +
            $"Időpont: {reservation.BookingFrom.ToString("yyyy. MM. dd.")} – {reservation.BookingTo.ToString("yyyy. MM. dd.")} <br>" +
            $"Szoba: {reservation.Room.Name} <br>" +
            $"Vendég: {reservation.ApplicationUser.LastName} {reservation.ApplicationUser.FirstName}, email: {reservation.ApplicationUser.Email}"
        };

        return notification;
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
            smtp.CheckCertificateRevocation = false;
            smtp.ServerCertificateValidationCallback = MySslCertificateValidationCallback!;
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

    bool MySslCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // If there are no errors, then everything went smoothly.
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        // Note: MailKit will always pass the host name string as the `sender` argument.
        var host = (string)sender;

        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != 0)
        {
            // This means that the remote certificate is unavailable. Notify the user and return false.
            _logger.LogWarning("The SSL certificate was not available for {0}", host);
            return false;
        }

        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
        {
            // This means that the server's SSL certificate did not match the host name that we are trying to connect to.
            var certificate2 = certificate as X509Certificate2;
            var cn = certificate2 != null ? certificate2.GetNameInfo(X509NameType.SimpleName, false) : certificate.Subject;

            _logger.LogWarning("The Common Name for the SSL certificate did not match {0}. Instead, it was {1}.", host, cn);
            return false;
        }

        // The only other errors left are chain errors.
        _logger.LogWarning("The SSL certificate for the server could not be validated for the following reasons:");

        // The first element's certificate will be the server's SSL certificate (and will match the `certificate` argument)
        // while the last element in the chain will typically either be the Root Certificate Authority's certificate -or- it
        // will be a non-authoritative self-signed certificate that the server admin created. 
        foreach (var element in chain.ChainElements)
        {
            // Each element in the chain will have its own status list. If the status list is empty, it means that the
            // certificate itself did not contain any errors.
            if (element.ChainElementStatus.Length == 0)
                continue;

            _logger.LogWarning("\u2022 {0}", element.Certificate.Subject);
            foreach (var error in element.ChainElementStatus)
            {
                // `error.StatusInformation` contains a human-readable error string while `error.Status` is the corresponding enum value.
                _logger.LogWarning("\t\u2022 {0}", error.StatusInformation);
            }
        }

        return false;
    }

}
