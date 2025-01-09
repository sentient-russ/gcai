using gcai.Models;
using gcia.Controllers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace gcai.Services
{
    public class EmailSender
    {

        private string? emailPass = Environment.GetEnvironmentVariable("GC_Email_Pass");
        private string? emailAddress = Environment.GetEnvironmentVariable("GC_Email_Address");
        private string? emailServer = Environment.GetEnvironmentVariable("GC_Email_Server");

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
            smtp.Connect(emailServer, 465, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailPass);
            var response = smtp.Send(email);
            smtp.Disconnect(true);
            return response;
        }
    }
}
