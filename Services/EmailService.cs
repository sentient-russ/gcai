using gcai.Models;
using gcia.Controllers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace gcai.Services
{
    public class EmailService
    {
        private string GC_Email_Pass = Environment.GetEnvironmentVariable("GC_Email_Pass");
        public String SendContactMessage(ContactDataModel complexDataIn)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("cs@magnadigi.com"));
            email.To.Add(MailboxAddress.Parse("cs@magnadigi.com"));
            email.Subject = "GCIA Contact";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<p><strong>Contact Name: </strong>" + complexDataIn.Name + "</p>" +
              "<p><strong>Contact Email: </strong>" + complexDataIn.Email + "</p>" +
              "<p><strong>Message: </strong>" + complexDataIn.Message + "</p>"
            };
            using var smtp = new SmtpClient();
            smtp.Connect("us2.smtp.mailhostbox.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("cs@magnadigi.com", GC_Email_Pass);
            var response = smtp.Send(email);
            smtp.Disconnect(true);
            return response;
        }
    }
}
