using Hotel.Backend.WebAPI.Abstractions.Services;
using Hotel.Backend.WebAPI.Models.DTO;
using Hotel.Backend.WebAPI.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Net;

namespace Hotel.Backend.WebAPI.Controllers;

[Route("hotel/[controller]")]
[ApiController]
public class EmailsController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailsController> _logger;

    public EmailsController(IEmailService emailService, ILogger<EmailsController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> SendEmail(EmailDTO message)
    {
        try
        {
            await _emailService.SendEmailAsync(message);
            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }

    }


}
