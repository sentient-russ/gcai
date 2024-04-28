using gcai.Areas.Identity.Services;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MySql.Data;
using Microsoft.Extensions.Logging;
using gcia.Areas.Identity.Services;

namespace gcai.Areas.Identity.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private string GC_Email_Pass;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        GC_Email_Pass = Environment.GetEnvironmentVariable("GC_Email_Pass");
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }
    //updated to smtp from sendgrid 4/24/2023
    public async Task Execute(string subject, string message, string toEmail)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("cs@magnadigi.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        using var smtp = new SmtpClient();
        smtp.Connect("us2.smtp.mailhostbox.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("cs@magnadigi.com", GC_Email_Pass);
        var response = smtp.Send(email);
        smtp.Disconnect(true);
        _logger.LogInformation("The message smtp send to " + toEmail + "was attempted and returned a status of: " + response);
    }
}



